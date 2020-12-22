using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfMailSenderScheduler.Commands;
using WpfMailSenderScheduler.Data;
using WpfMailSenderLibrary.Interfaces; 
using WpfMailSenderScheduler.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls; 
using WpfMailSenderLibrary;
using EFMailsAndSendersDb.Data;
using Microsoft.EntityFrameworkCore;
using System.Windows.Threading;

namespace WpfMailSenderScheduler.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly IMailService _mailService;
        private readonly MailsAndSendersDbModel _mailsAndSendersDb;


        public MainWindowViewModel(IMailService mailService, MailsAndSendersDbModel mailsAndSendersDb)
        {
            _mailService = mailService;
            _mailsAndSendersDb = mailsAndSendersDb; 
        }

        public string Title { get; set; } = "Рассыльщик почты";
        private ObservableCollection<Server> _servers;
        public ObservableCollection<Server> Servers
        {
            get { return _servers; }
            set => Set(ref _servers, value); 
        }

        public string CountServers => (Servers?.Count??0).ToString();

        private ObservableCollection<Sender> _senders;
        public ObservableCollection<Sender> Senders
        {
            get { return _senders; }
            set => Set(ref _senders, value);
        }

        public string CountSenders => (Senders?.Count ?? 0).ToString();


        private ObservableCollection<Recipient> _recipients;
        public ObservableCollection<Recipient> Recipients
        {
            get { return _recipients; }
            set => Set(ref _recipients, value);
        }

        public string CountRecipients => (Recipients?.Count ?? 0).ToString();

        private ObservableCollection<Message> _messages;
        public ObservableCollection<Message> Messages
        {
            get { return _messages; }
            set => Set(ref _messages, value);
        }

        private Sender _selectedSender;
        public Sender SelectedSender
        {
            get => _selectedSender;  
            set => Set(ref _selectedSender, value);
        }

        private Server _selectedServer;
        public Server SelectedServer
        {
            get => _selectedServer;
            set => Set(ref _selectedServer, value);
        }

        private Recipient _selectedRecipient;
        public Recipient SelectedRecipient
        {
            get => _selectedRecipient;
            set => Set(ref _selectedRecipient, value);
        }

        private Message _selectedMessage = new Message();
        public Message SelectedMessage
        {
            get => _selectedMessage;
            set => Set(ref _selectedMessage, value);
        }

        private TabItem _selectedTabItem;
        public TabItem SelectedTabItem
        {
            get => _selectedTabItem;
            set => Set(ref _selectedTabItem, value);
        }

        private DateTime? _selectedCalendarDate = DateTime.Now;
        public DateTime? SelectedCalendarDate
        {
            get => _selectedCalendarDate;
            set
            {
                Set(ref _selectedCalendarDate, value);
                /*для demo*/
                if (SelectedTask != null && value.HasValue)
                {
                    SelectedTask.SendDate = (DateTime)value;
                    OnPropertyChanged("SelectedTask");
                }
            }
        }

        private SenderTask _selectedTask;
        public SenderTask SelectedTask {
            get => _selectedTask;
            set => Set(ref _selectedTask, value);
        }
         
        private ObservableCollection<SenderTask> _senderTasks = new ObservableCollection<SenderTask>();
        public ObservableCollection<SenderTask> SenderTasks
        {
            get { return _senderTasks; }
            set => Set(ref _senderTasks, value);
        }
                                        

        private ICommand sendMailMessageCommand;
        public ICommand SendMailMessageCommand => sendMailMessageCommand ?? (sendMailMessageCommand = new RelayCommand((object par) => {
            if (SelectedTask == null)
            {
                App.ShowDialogError("Нeвыбрано задание");
                return;
            }
            if (SelectedTask.Server == null || string.IsNullOrWhiteSpace(SelectedTask.Server.Address) || string.IsNullOrWhiteSpace(SelectedTask.Server.Login) || string.IsNullOrWhiteSpace(SelectedTask.Server.Password))
            {
                App.ShowDialogError("Некорректные данные для отправки smpt-сервером! \n\rПроверьте параметры: адрес, логин, пароль.");
                return;
            }
            if (SelectedTask.Message.Sender == null || string.IsNullOrWhiteSpace(SelectedTask.Message.Sender.Address))
            {
                App.ShowDialogError("Отправитель не задан или некорректные данные адреса");
                return;
            }
            if (SelectedTask.Message.Recipient == null || string.IsNullOrWhiteSpace(SelectedTask.Message.Recipient.Address))
            {
                App.ShowDialogError("Получатель не задан или некорректные данные адреса");
                return;
            }
            if (string.IsNullOrWhiteSpace(SelectedTask.Message.Subject))
            {
                App.ShowDialogError("Пустая тема сообщения");
                return;
            }
            var xamlTextBody = string.IsNullOrWhiteSpace(SelectedTask.Message.Body) ? null : HTMLConverter.HtmlToXamlConverter.ConvertHtmlToXaml(SelectedTask.Message.Body, false);
            var plainTextBody = string.IsNullOrWhiteSpace(xamlTextBody) ? null : Converters.XamlToPlainTextConverter.ConvertRtfToXaml(xamlTextBody);
            if (string.IsNullOrWhiteSpace(plainTextBody))
            {
                App.ShowDialogError("Пустое тело сообщения");
                SelectedMessage = new Message
                {
                    Subject = SelectedTask.Message.Subject,
                    Sender = SelectedTask.Message.Sender,
                    SenderId = SelectedTask.Message.Sender.Id,
                    Recipient = SelectedTask.Message.Recipient,
                    RecipientId = SelectedTask.Message.Recipient.Id
                };
                GotoTabCommand.Execute(par);//перейти во вкладку для редактирования тела письма 
                return;
            }

            var sender = SelectedTask.Message.Sender;
            var serverName = SelectedTask.Message.Sender.Address;
            var server  = SelectedTask.Server;
            var client = _mailService.GetSender(server.Address, server.Port, true, server.Login, server.Password);
            var recipient = SelectedTask.Message.Recipient;
            var subject = SelectedTask.Message.Subject;
            var body = SelectedTask.Message.Body;

            try
            {
                client.Send(sender.Address, recipient.Address, subject, body, true);
                App.ShowDialogInfo("Сообщение успешно отправлено!");
            }
            catch(Exception ex)
            {
                App.ShowDialogError(ex);
            }
        }, (object p) => { return true; }
        ));

        private ICommand saveTaskCommand;
        public ICommand SaveTaskCommand => saveTaskCommand ?? (saveTaskCommand = new RelayCommand((object par) => {

            if ((SenderTasks?.Count??0) == 0)
            {
                App.ShowDialogError("Список заданий пуст");
                return;
            }
            foreach (var task in SenderTasks)
            {
                if (task.IsProcessed) continue;

                task.IsSendEnd = false;
                task.Error = null;
                task.IsProcessed = true;
            }

            var sc = new SchedulerClass();
            var list = SenderTasks.Where(x=>!x.IsProcessed).ToList();
            if (list == null || list.Count == 0) return;

            sc.SendTaskAsync(list, _mailService).ContinueWith(x=>
            {
                list.ForEach(l =>
                {
                    var task = _mailsAndSendersDb.SenderTasks.FirstOrDefault(t=> t.Id == l.Id);
                    task.SendDate = l.SendDate;
                    task.IsSendEnd = l.IsSendEnd;
                    task.IsSuccessful = string.IsNullOrWhiteSpace(l.Error);
                    task.Error = l.Error;
                    task.Attempt++;
                    task.IsProcessed = false;
                });
                _mailsAndSendersDb.SaveChangesAsync();

                SelectedTabItem.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() => {
                        App.ShowDialogInfo($"Выполнение заданий завешено! Выполнено {list.Sum(s => s.IsSendEnd ? 1 : 0)} заданий.");
                        SenderTasks = new ObservableCollection<SenderTask>(list);
                        OnPropertyChanged("SenderTasks");

                        SelectedTask = SenderTasks.FirstOrDefault();
                        OnPropertyChanged("SelectedTask");

                    }) );  
            });

            SenderTasks = new ObservableCollection<SenderTask>(list);
            OnPropertyChanged("SenderTasks");

            SelectedTask = SenderTasks.FirstOrDefault();
            OnPropertyChanged("SelectedTask");
        }));        
        
    }
}
