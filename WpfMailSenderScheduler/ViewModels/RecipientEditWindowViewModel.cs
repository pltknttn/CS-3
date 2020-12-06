using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfMailSenderScheduler.Commands;
using WpfMailSenderScheduler.Models;

namespace WpfMailSenderScheduler.ViewModels
{
    public class RecipientEditWindowViewModel : ViewModelBase
    {
        private bool _canEdit = true;
        public bool CanEdit { get => _canEdit; set => _canEdit = value; }

        private string _title = "Редактирование получателя";
        public string Title { get => _title; set => Set(ref _title, value); }
         
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Address { get; set; } 

        public RecipientEditWindowViewModel() : this(null) { }
        public RecipientEditWindowViewModel(Recipient recipient, Predicate<Recipient> saveFunc = null)
        {
            _saveFunc = saveFunc;
            if (recipient != null)
            {
                Id = recipient.Id;
                Name = recipient.Name;
                Address = recipient.Address; 
                Title = $"Редактирование получателя {recipient.FullName}";
            }
        }

        private bool canClose = true;
        private bool? _dialogResult = null;
        public bool? DialogResult
        {
            get => _dialogResult;
            set => Set(ref _dialogResult, value);
        }

        private Predicate<Recipient> _saveFunc;
        private ICommand doOkCommand;
        public ICommand DoOkCommand => doOkCommand ?? (doOkCommand = new RelayCommand(() => {

            var recipient = new Recipient { Id = this.Id, Name = this.Name, Address = this.Address};
            if (_saveFunc?.Invoke(recipient) ?? false) DialogResult = true;
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
