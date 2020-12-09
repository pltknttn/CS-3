using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfMailSenderLibrary.Models;
using WpfMailSenderScheduler.Commands;
using WpfMailSenderScheduler.Data.ValidationRules;

namespace WpfMailSenderScheduler.ViewModels
{
    public class TaskEditWindowViewModel : ViewModelBase
    {
        private bool _canEdit = true;
        public bool CanEdit { get => _canEdit; set => _canEdit = value; }

        private string _title = "Редактирование задания";
        public string Title { get => _title; set => Set(ref _title, value); }

        public int Id { get; private set; }

        private DateTime _sendDate = DateTime.Now;
        public DateTime SendDate { get => _sendDate; set => Set(ref _sendDate, value); }

        private bool _canEditSendDate = true;
        public bool CanEditSendDate { get => _canEditSendDate & _canEdit; set => _canEditSendDate = value & _canEdit; }

        private Recipient _recipient;
        public Recipient Recipient
        {
            get => _recipient;
            set
            {
                Set(ref _recipient, value);
                RemoveError("Recipient");
                if (Recipient != null && !UtilValidation.ValidateAddress(value?.Address, out var _)) 
                    AddError("Recipient", "Некорректный адрес электронной почты");
            }
        }

        private Sender _sender;
        public Sender Sender
        {
            get => _sender;
            set
            {
                Set(ref _sender, value);
                RemoveError("Sender");
                if (Recipient != null && !UtilValidation.ValidateAddress(value?.Email, out var _))
                    AddError("Sender", "Некорректный адрес электронной почты");
            }
        }

        private ObservableCollection<Sender> _senders;
        public ObservableCollection<Sender> Senders
        {
            get { return _senders; }
            set => Set(ref _senders, value);
        }
         
        private ObservableCollection<Recipient> _recipients;
        public ObservableCollection<Recipient> Recipients
        {
            get { return _recipients; }
            set => Set(ref _recipients, value);
        }


        public TaskEditWindowViewModel() : this(null) { }
        public TaskEditWindowViewModel(SenderTask task, Predicate<SenderTask> saveFunc = null)
        {
            _saveFunc = saveFunc;
            if (task != null)
            {
                Id = task.Id;
                Recipient = task.Recipient;
                Sender = task.Sender;
                SendDate = task.SendDate;
                Subject = task.Subject;
                Body = task.Body;
                Title = task.Id > 0 ? $"Редактирование задания № {task.Id}" : "Добавление задания";
            }
        }

        private bool replySave = false;

        private string _subject;
        public string Subject { get => _subject;
            set
            {
                Set(ref _subject, value);
                RemoveError("Subject");
                if (replySave && string.IsNullOrWhiteSpace(_subject))
                    AddError("Subject", "Укажите тему!");
            }
        }


        private string _body;
        public string Body
        {
            get => _body; set
            {
                Set(ref _body, value);
                RemoveError("Body");
                if (replySave && string.IsNullOrWhiteSpace(Converters.RtfToPlainTextConverter.ConvertRtfToPlainText(value)))
                    AddError("Body", "Укажите тело!");
            }
        }

        private bool canClose = true;
        private bool? _dialogResult = null;
        public bool? DialogResult
        {
            get => _dialogResult;
            set => Set(ref _dialogResult, value);
        }

        private Predicate<SenderTask> _saveFunc;
        private ICommand doOkCommand;
        public ICommand DoOkCommand => doOkCommand ?? (doOkCommand = new RelayCommand(() => 
        {
            replySave = true;
            DialogResult = null;
            if (Sender == null)
            {
                AddError("Sender", "Выберите отправителя");
                App.ShowDialogError("Выберите отправителя");
                return;
            }
            if (Recipient == null)
            {
                AddError("Recipient", "Выберите получателя");
                App.ShowDialogError("Выберите получателя");                
                return;
            }
            if (string.IsNullOrWhiteSpace(Subject))
            {
                AddError("Body", "Укажите тему!");
                App.ShowDialogError("Укажите тему!");
                return;
            }
            if (string.IsNullOrWhiteSpace(Converters.RtfToPlainTextConverter.ConvertRtfToPlainText(Body)))
            {
                AddError("Body", "Укажите тело!");
                App.ShowDialogError("Укажите тело!");
                return;
            }
            if (HasErrors)
            {
                App.ShowDialogError("Исправьте ошибки ввода!");                
                return;
            }
            var senderTask = new SenderTask { Id = this.Id, Body = this.Body, Subject = this.Subject, Recipient = this.Recipient, Sender = this.Sender, SendDate = this.SendDate};
            if (_saveFunc?.Invoke(senderTask) ?? false) DialogResult = true;
        }));

        private ICommand doCancelCommand;
        public ICommand DoCancelCommand => doCancelCommand ?? (doCancelCommand = new RelayCommand(() => {
            DialogResult = false;
        }));

        private ICommand windowClosingCommand;
        public ICommand WindowClosingCommand => windowClosingCommand ?? (windowClosingCommand = new RelayCommand<CancelEventArgs>((args) => {
            args.Cancel = !canClose;
        }));

        private ICommand loadDataCommand;
        public ICommand LoadDataCommand => loadDataCommand ?? (loadDataCommand = new RelayCommand((object par) =>
        {
        })); 
    }
}
