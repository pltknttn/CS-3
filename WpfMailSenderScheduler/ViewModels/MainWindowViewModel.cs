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
using WpfMailSenderLibrary.Models;
using WpfMailSenderScheduler.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using WpfMailSenderLibrary.Interfaces;
using WpfMailSenderLibrary;

namespace WpfMailSenderScheduler.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IMailService _mailService;
        private readonly ISendersStorage _sendersStorage;
        private readonly IServersStorage _serversStorage;
        private readonly IMessagesStorage _messagesStorage;
        private readonly IRecipientsStorage _recipientsStorage;


        public MainWindowViewModel(IMailService mailService, 
            IServersStorage serversStorage, ISendersStorage sendersStorage, 
            IRecipientsStorage recipientsStorage, IMessagesStorage messagesStorage)
        {
            _mailService = mailService;
            _sendersStorage = sendersStorage;
            _serversStorage = serversStorage;
            _recipientsStorage = recipientsStorage;
            _messagesStorage = messagesStorage;
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

        private Message _selectedMessage;
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

        /// <summary>
        /// Загрузка
        /// </summary>
        private ICommand loadDataCommand;
        public ICommand LoadDataCommand => loadDataCommand ?? (loadDataCommand = new RelayCommand((object par) =>
        {
            _serversStorage.Load();
            _sendersStorage.Load();
            _recipientsStorage.Load();
            _messagesStorage.Load();

            Servers = new ObservableCollection<Server>(_serversStorage.Items);
            Senders = new ObservableCollection<Sender>(_sendersStorage.Items);
            Recipients = new ObservableCollection<Recipient>(_recipientsStorage.Items);
            Messages = new ObservableCollection<Message>(_messagesStorage.Items);

            OnPropertyChanged("CountServers");
            OnPropertyChanged("CountSenders");
            OnPropertyChanged("CountRecipients"); 
        }));

        /// <summary>
        /// Сохранить результаты
        /// </summary>
        private ICommand saveDataCommand;
        public ICommand SaveDataCommand => saveDataCommand ?? (saveDataCommand = new RelayCommand((object par) => { 
            _serversStorage.SaveChanges();
            _sendersStorage.SaveChanges();
            _recipientsStorage.SaveChanges();
            _messagesStorage.SaveChanges();

        }));

        /// <summary>
        /// Создать сервер
        /// </summary>
        private ICommand createServerDataCommand;
        public ICommand CreateServerDataCommand => createServerDataCommand ?? (createServerDataCommand = new RelayCommand((object par) => {

            var serverWindow = new ServerEditWindow()
            {
                DataContext = new ServerEditWindowViewModel(null, (s) => {
                    s.Id = Servers.DefaultIfEmpty().Max(x => x?.Id ?? 0) + 1;
                    Servers.Add(s);
                    _serversStorage.Items.Add(s);
                    SelectedServer = s;
                    return true;
                })
            };
            serverWindow.ShowDialog();
            OnPropertyChanged("CountServers"); 
        }));

        private ICommand editServerDataCommand;
        public ICommand EditServerDataCommand => editServerDataCommand ?? (editServerDataCommand = new RelayCommand((object par) => {
            
            if (!(par is Server server))
            {
                App.Services.GetService<IDialogService>().ShowWarn("Выберите  smtp-сервер");
                return;
            }
            var serverWindow = new ServerEditWindow()
            {
                DataContext = new ServerEditWindowViewModel(server, (s) => {
                    var indx = Servers.IndexOf(server);
                    Servers.RemoveAt(indx); 
                    Servers.Insert(indx, s); 
                    _serversStorage.Items.Remove(server);
                    _serversStorage.Items.Add(s);
                    SelectedServer = s;
                    return true;
                }) 
            };
            serverWindow.ShowDialog();
            OnPropertyChanged("CountServers"); 
        }));
        private ICommand deleteServerDataCommand;
        public ICommand DeleteServerDataCommand => deleteServerDataCommand ?? (deleteServerDataCommand = new RelayCommand((object par) => {
            
            if (!(par is Server server))
            {
                App.Services.GetService<IDialogService>().ShowWarn("Выберите  smtp-сервер");
                return;
            }
            var serverWindow = new ServerEditWindow()
            {
                DataContext = new ServerEditWindowViewModel(server, (s) =>
                {
                    Servers.Remove(server);
                    _serversStorage.Items.Remove(server);
                    SelectedServer = Servers.FirstOrDefault();
                    return true;
                })
                { CanEdit = false } 
            };
            serverWindow.ShowDialog();
            OnPropertyChanged("CountServers"); 
        }));

        /// <summary>
        /// Создать отправителя
        /// </summary>
        private ICommand createSenderDataCommand;
        public ICommand CreateSenderDataCommand => createSenderDataCommand ?? (createSenderDataCommand = new RelayCommand((object par) => {
            var senderWindow = new SenderEditWindow()
            {
                DataContext = new SenderEditWindowViewModel(null, (s) => {
                    s.Id = Senders.DefaultIfEmpty().Max(x => x?.Id??0)  + 1;
                    Senders.Add(s);
                    _sendersStorage.Items.Add(s);
                    SelectedSender = s;
                    return true;
                })
                {
                    Servers = this.Servers
                }
            };
            senderWindow.ShowDialog(); 
            OnPropertyChanged("CountSenders"); 
        }));

        private ICommand editSenderDataCommand;
        public ICommand EditSenderDataCommand => editSenderDataCommand ?? (editSenderDataCommand = new RelayCommand((object par) => {
            if (!(par is Sender sender))
            {
                App.Services.GetService<IDialogService>().ShowWarn("Выберите отправителя");
                return;
            }
            var senderWindow = new SenderEditWindow()
            {
                DataContext = new SenderEditWindowViewModel(sender, (s) => {
                    var indx = Senders.IndexOf(sender);
                    Senders.RemoveAt(indx);
                    Senders.Insert(indx, s);
                    _sendersStorage.Items.Remove(sender);
                    _sendersStorage.Items.Add(s);
                    SelectedSender = s;
                    return true;
                })
                {
                    Servers = this.Servers
                }
            };
            senderWindow.ShowDialog(); 
            OnPropertyChanged("CountSenders"); 
        }));
        
        private ICommand deleteSenderDataCommand;
        public ICommand DeleteSenderDataCommand => deleteSenderDataCommand ?? (deleteSenderDataCommand = new RelayCommand((object par) => {
            if (!(par is Sender sender))
            {
                App.Services.GetService<IDialogService>().ShowWarn("Выберите отправителя");
                return;
            }
            var senderWindow = new SenderEditWindow()
            {
                DataContext = new SenderEditWindowViewModel(sender, (s) => {
                    Senders.Remove(sender);
                    _sendersStorage.Items.Remove(sender);
                    SelectedSender = Senders.FirstOrDefault();
                    return true; })
                {
                    Servers = this.Servers,
                    CanEdit = false
                }
            };
            senderWindow.ShowDialog(); 
            OnPropertyChanged("CountSenders"); 

        }));

        private ICommand createRecipientDataCommand;
        public ICommand CreateRecipientDataCommand => createRecipientDataCommand ?? (createRecipientDataCommand = new RelayCommand((object par) => {
            var recipientWindow = new RecipientEditWindow()
            {
                DataContext = new RecipientEditWindowViewModel(null, (s) => {
                    s.Id = Recipients.DefaultIfEmpty().Max(x => x?.Id ?? 0) + 1;
                    Recipients.Add(s);
                    _recipientsStorage.Items.Add(s);
                    SelectedRecipient = s;
                    return true;
                })
            };
            recipientWindow.ShowDialog(); 
            OnPropertyChanged("CountRecipients");
        }));

        private ICommand editRecipientDataCommand;
        public ICommand EditRecipientDataCommand => editRecipientDataCommand ?? (editRecipientDataCommand = new RelayCommand((object par) => {
            if (!(par is Recipient recipient))
            {
                App.ShowDialogWarn("Выберите адресат\\получателя");
                return;
            }
            var recipientWindow = new RecipientEditWindow()
            {
                DataContext = new RecipientEditWindowViewModel(recipient, (s) => {
                    var indx = Recipients.IndexOf(recipient);
                    Recipients.RemoveAt(indx);
                    Recipients.Insert(indx, s);
                    _recipientsStorage.Items.Remove(recipient);
                    _recipientsStorage.Items.Add(s);
                    SelectedRecipient = s;
                    return true;
                })
            };
            recipientWindow.ShowDialog();
            OnPropertyChanged("CountRecipients");
            OnPropertyChanged("CountRecipients");
        }));
        
        private ICommand deleteRecipientDataCommand;
        public ICommand DeleteRecipientDataCommand => deleteRecipientDataCommand ?? (deleteRecipientDataCommand = new RelayCommand((object par) => {
            if (!(par is Recipient recipient))
            {
                App.Services.GetService<IDialogService>().ShowWarn("Выберите адресат\\получателя");
                return;
            }
            var recipientWindow = new RecipientEditWindow()
            {
                DataContext = new RecipientEditWindowViewModel(recipient, (s) => { 
                    Recipients.Remove(recipient);
                    _recipientsStorage.Items.Remove(recipient);
                    SelectedRecipient = Recipients.FirstOrDefault();
                    return true;
                })
                { CanEdit = false }
            };
            recipientWindow.ShowDialog();
            OnPropertyChanged("CountRecipients");
        }));

        private ICommand saveMessageDataCommand;
        public ICommand SaveMessageDataCommand => saveMessageDataCommand ?? (saveMessageDataCommand = new RelayCommand((object par) => {
            var message = SelectedMessage ?? new Message();
            if (string.IsNullOrWhiteSpace(message.Subject))
            {
                App.ShowDialogWarn("Пустая тема письма!");
                return;
            }
            var plainTextBody = Converters.RtfToPlainTextConverter.ConvertRtfToPlainText(message.Body);
            if (string.IsNullOrWhiteSpace(plainTextBody?.Trim()?.Replace("\n\r", "").Trim()))
            {
                App.ShowDialogWarn("Пустое тело письма!");
                return;
            }

            var oldMsg = Messages.FirstOrDefault(x=>x.Id == message.Id);
            if (oldMsg == null)
            {
                message.Id = Messages.DefaultIfEmpty().Max(x => x?.Id ?? 0) + 1;
                Messages.Add(message);
                _messagesStorage.Items.Add(message);
                return;
            }

            var index = Messages.IndexOf(oldMsg);
            Messages.RemoveAt(index);
            Messages.Insert(index, message);
            _messagesStorage.Items.Remove(oldMsg);
            _messagesStorage.Items.Add(message);
        }));

        private ICommand switchNextCommand;
        public ICommand SwitchNextCommand => switchNextCommand ?? (switchNextCommand = new RelayCommand((object par) => {
            var count = Recipients.Count;
            if (count > 0)
            {
                var index = Recipients.IndexOf(SelectedRecipient?? Recipients.FirstOrDefault());
                index = index + 1 >= count ? 0 : index + 1;
                SelectedRecipient = Recipients[index];
            }
        }));
        
        private ICommand switchPreviosCommand;
        public ICommand SwitchPreviosCommand => switchPreviosCommand ?? (switchPreviosCommand = new RelayCommand((object par) => {
            var count = Recipients.Count;
            if (count > 0)
            {
                var index = Recipients.IndexOf(SelectedRecipient ?? Recipients.FirstOrDefault());
                index = index - 1 < 0 ? count - 1 : index - 1;
                SelectedRecipient = Recipients[index];
            }
        }));

        private ICommand gotoTabCommant;
        public ICommand GotoTabCommand => gotoTabCommant ?? (gotoTabCommant = new RelayCommand((object par) => {
            if (par is TabItem tab)
            {
                SelectedTabItem = tab;
            }
        }));

        private ICommand selectedRecipientCommand;
        public ICommand SelectedRecipientCommand => selectedRecipientCommand ?? (selectedRecipientCommand = new RelayCommand((object par) => {
            if (par is SelectionChangedEventArgs args && args.Source is DataGrid grid)
            {
                grid.Focus(); 
            }
        }));

        private ICommand sendMailMessageCommand;
        public ICommand SendMailMessageCommand => sendMailMessageCommand ?? (sendMailMessageCommand = new RelayCommand((object par) => {
            if (SelectedTask == null)
            {
                App.ShowDialogError("Нeвыбрано задание");
                return;
            }
            if (SelectedTask.Sender == null || string.IsNullOrWhiteSpace(SelectedTask.Sender.Address) || string.IsNullOrWhiteSpace(SelectedTask.Sender.Login) || string.IsNullOrWhiteSpace(SelectedTask.Sender.Password))
            {
                App.ShowDialogError("Отправитель не задан или некорректные данные для отправки smpt-сервером! \n\rПроверьте параметры: адрес, логин, пароль.");
                return;
            }
            if (SelectedTask.Recipient == null || string.IsNullOrWhiteSpace(SelectedTask.Recipient.Address))
            {
                App.ShowDialogError("Получатель не задан или некорректные данные адреса");
                return;
            }
            if (string.IsNullOrWhiteSpace(SelectedTask.Subject))
            {
                App.ShowDialogError("Пустая тема сообщения");
                return;
            }
            var xamlTextBody = string.IsNullOrWhiteSpace(SelectedTask.Body) ? null : HTMLConverter.HtmlToXamlConverter.ConvertHtmlToXaml(SelectedTask.Body, false);
            var plainTextBody = string.IsNullOrWhiteSpace(xamlTextBody) ? null : Converters.XamlToPlainTextConverter.ConvertRtfToXaml(xamlTextBody);
            if (string.IsNullOrWhiteSpace(plainTextBody))
            {
                App.ShowDialogError("Пустое тело сообщения");
                SelectedMessage = new Message { Subject = SelectedTask.Subject };
                GotoTabCommand.Execute(par);//перейти во вкладку для редактирования тела письма 
                return;
            }

            var sender = SelectedTask.Sender;
            var serverName = SelectedTask.Sender.Address;
            var server  = Servers.FirstOrDefault(x=>x.Address == serverName);
            var client = _mailService.GetSender(server.Address, server.Port, true, sender.Login, sender.Password);
            var recipient = SelectedTask.Recipient;
            var subject = SelectedTask.Subject;
            var body = SelectedTask.Body;

            try
            {
                client.Send($"{sender.Login}@{serverName}", recipient.Address, subject, body, true);
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
                task.IsSendEnd = false;
                task.ErrorSend = null;
            }
            var sc = new SchedulerClass();
            var list = SenderTasks.ToList();
            sc.SendTask(list, _mailService);
            App.ShowDialogInfo($"Выполнение заданий завешено! Выполнено {list.Sum(s=>s.IsSendEnd ? 1 : 0)} заданий.");

            SenderTasks = new ObservableCollection<SenderTask>(list);
            OnPropertyChanged("SenderTasks");

            SelectedTask = SenderTasks.FirstOrDefault();
            OnPropertyChanged("SelectedTask"); 
        }));

        private ICommand undoTaskCommand;
        public ICommand UndoTaskCommand => undoTaskCommand ?? (undoTaskCommand = new RelayCommand((object par) => {
            SenderTasks.Clear();
        }));

        private void AddTemplateTask()
        {
            /*для demo */
            var count = SenderTasks.Count + 1;
            var date = SelectedCalendarDate ?? DateTime.Now;
            var newTask = new SenderTask { 
                TaskDate = date.AddDays(-1), 
                SendDate = date, 
                Subject = $"Тема сообщения № {count} от {date:dd.MM.yyyy hh:mm:ss}", 
                Body = "Тело сообщения", 
                Recipient = SelectedRecipient, 
                Sender = SelectedSender,
                Server = Servers?.FirstOrDefault(x => x.Address == SelectedSender?.Address)
            };      
            SenderTasks.Add(newTask);             
            SelectedTask = SenderTasks.LastOrDefault();           
        }

        private ICommand _selectionTabChangedCommand; 
        public ICommand SelectionTabChangedCommand => _selectionTabChangedCommand ?? (_selectionTabChangedCommand = new RelayCommand((object par) => {
            if (par is SelectionChangedEventArgs args && args.Source is TabControl tab && (tab.SelectedItem as TabItem)?.Name == "tabScheduler")
            {
                /*для demo */
                //AddTemplateTask();
            }
        }));

        private ICommand _saveMessageCommand;
        public ICommand SaveMessageCommand => _saveMessageCommand ?? (_saveMessageCommand = new RelayCommand((object par) => {
            App.ShowDialogInfo("Нажата кнопка сохранить сообщение");
        }));

        private ICommand addNewTaskCommand;
        public ICommand AddNewTaskCommand => addNewTaskCommand ?? (addNewTaskCommand = new RelayCommand((object par) => {
            AddTemplateTask();
        }));

        private ICommand removeTaskCommand;
        public ICommand RemoveTaskCommand => removeTaskCommand ?? (removeTaskCommand = new RelayCommand((object par) => {
            SenderTasks.Remove(SelectedTask);
            SelectedTask = SenderTasks.LastOrDefault();
        }));

        private ICommand _editTaskCommand;
        public ICommand EditTaskCommand => _editTaskCommand ?? (_editTaskCommand = new RelayCommand((object par) => {
            if (SelectedTask == null)
            {
                App.ShowDialogWarn("Выберите задание");
                return;
            }
            var senderTask = SelectedTask;
            var senderWindow = new TaskEditWindow()
            {
                DataContext = new TaskEditWindowViewModel(senderTask, (s) => {
                    var indx = SenderTasks.IndexOf(senderTask);
                    s.Server = Servers?.FirstOrDefault(x => x.Address == s.Sender.Address);
                    SenderTasks.RemoveAt(indx);
                    SenderTasks.Insert(indx, s);
                    SelectedTask = s;
                    return true;
                })
                {
                    Recipients = this.Recipients,
                    Senders = this.Senders,
                    CanEditSendDate = false
                }
            };
            senderWindow.ShowDialog(); 
        }));

        /*
         private ICommand _templateCommand;
         public ICommand TemplateCommand => _templateCommand ?? (_templateCommand = new RelayCommand((object par) => {
            //команда

         })); 
         */
    }
}
