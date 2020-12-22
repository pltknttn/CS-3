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

namespace WpfMailSenderScheduler.ViewModels
{
    public partial class MainWindowViewModel   
    {
        /// <summary>
        /// Загрузка
        /// </summary>
        private ICommand loadDataCommand;
        public ICommand LoadDataCommand => loadDataCommand ?? (loadDataCommand = new RelayCommand((object par) =>
        { 
            Servers = new ObservableCollection<Server>(_mailsAndSendersDb.Servers);
            Senders = new ObservableCollection<Sender>(_mailsAndSendersDb.Senders);
            Recipients = new ObservableCollection<Recipient>(_mailsAndSendersDb.Recipients);
            Messages = new ObservableCollection<Message>(_mailsAndSendersDb.Messages.Include("Sender").Include("Recipient"));
            SenderTasks = new ObservableCollection<SenderTask>(_mailsAndSendersDb.SenderTasks.Include("Message"));

            OnPropertyChanged("CountServers");
            OnPropertyChanged("CountSenders");
            OnPropertyChanged("CountRecipients");
        }));

        /// <summary>
        /// Сохранить результаты
        /// </summary>
        private ICommand saveDataCommand;
        public ICommand SaveDataCommand => saveDataCommand ?? (saveDataCommand = new RelayCommand((object par) => {

        }));

        #region переключатель
        private ICommand switchNextCommand;
        public ICommand SwitchNextCommand => switchNextCommand ?? (switchNextCommand = new RelayCommand((object par) => {
            var count = Recipients.Count;
            if (count > 0)
            {
                var index = Recipients.IndexOf(SelectedRecipient ?? Recipients.FirstOrDefault());
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

        private ICommand _selectionTabChangedCommand;
        public ICommand SelectionTabChangedCommand => _selectionTabChangedCommand ?? (_selectionTabChangedCommand = new RelayCommand((object par) => {
            if (par is SelectionChangedEventArgs args && args.Source is TabControl tab && (tab.SelectedItem as TabItem)?.Name == "tabScheduler")
            {
                /*для demo */
                //AddTemplateTask();
            }
        }));

        #endregion

        #region сервер
        /// <summary>
        /// Создать сервер
        /// </summary>
        private ICommand createServerDataCommand;
        public ICommand CreateServerDataCommand => createServerDataCommand ?? (createServerDataCommand = new RelayCommand((object par) => {

            var serverWindow = new ServerEditWindow()
            {
                DataContext = new ServerEditWindowViewModel(null, (s) => {
                    _mailsAndSendersDb.Servers.Add(s);
                    _mailsAndSendersDb.SaveChanges();

                    Servers.Add(s);
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

                    var upd = _mailsAndSendersDb.Servers.FirstOrDefault(x => x.Id == s.Id);
                    upd.Login = s.Login;
                    upd.Password = s.Password;
                    upd.Port = s.Port;
                    upd.Address = s.Address;
                    upd.Name = s.Name;
                    _mailsAndSendersDb.SaveChanges();

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
                    var del = _mailsAndSendersDb.Servers.FirstOrDefault(x => x.Id == s.Id);
                    _mailsAndSendersDb.Entry(del).State = EntityState.Deleted;
                    _mailsAndSendersDb.Servers.Remove(del);
                    _mailsAndSendersDb.SaveChanges();

                    Servers.Remove(server);
                    SelectedServer = Servers.FirstOrDefault();
                    return true;
                })
                { CanEdit = false, Title = "Удалить?" }
            };
            serverWindow.ShowDialog();
            OnPropertyChanged("CountServers");
        }));
        #endregion

        #region отправитель
        /// <summary>
        /// Создать отправителя
        /// </summary>
        private ICommand createSenderDataCommand;
        public ICommand CreateSenderDataCommand => createSenderDataCommand ?? (createSenderDataCommand = new RelayCommand((object par) => {
            var senderWindow = new SenderEditWindow()
            {
                DataContext = new SenderEditWindowViewModel(null, (s) => {
                    _mailsAndSendersDb.Senders.Add(s);
                    _mailsAndSendersDb.SaveChanges();

                    Senders.Add(s);
                    SelectedSender = s;
                    return true;
                }) 
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
                    
                    var upd = _mailsAndSendersDb.Senders.FirstOrDefault(x => x.Id == s.Id); 
                    upd.Address = s.Address;
                    upd.Name = s.Name;
                    _mailsAndSendersDb.SaveChanges();

                    SelectedSender = s;
                    return true;
                }) 
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
                    
                    var del = _mailsAndSendersDb.Senders.FirstOrDefault(x => x.Id == s.Id);
                    _mailsAndSendersDb.Senders.Remove(del);
                    _mailsAndSendersDb.SaveChanges();

                    SelectedSender = Senders.FirstOrDefault();
                    return true;
                })
                { 
                    CanEdit = false,
                    Title = "Удалить?"
                }
            };
            senderWindow.ShowDialog();
            OnPropertyChanged("CountSenders");

        }));
        #endregion

        #region получатель
        private ICommand createRecipientDataCommand;
        public ICommand CreateRecipientDataCommand => createRecipientDataCommand ?? (createRecipientDataCommand = new RelayCommand((object par) => {
            var recipientWindow = new RecipientEditWindow()
            {
                DataContext = new RecipientEditWindowViewModel(null, (s) => {
                    _mailsAndSendersDb.Recipients.Add(s);
                    _mailsAndSendersDb.SaveChanges();

                    Recipients.Add(s);
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
                    
                    var upd = _mailsAndSendersDb.Recipients.FirstOrDefault(x => x.Id == s.Id);
                    upd.Address = s.Address;
                    upd.Name = s.Name;
                    _mailsAndSendersDb.SaveChanges();

                    SelectedRecipient = s;
                    return true;
                })
            };
            recipientWindow.ShowDialog(); 
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

                    var del = _mailsAndSendersDb.Recipients.FirstOrDefault(x => x.Id == s.Id);
                    _mailsAndSendersDb.Recipients.Remove(del);
                    _mailsAndSendersDb.SaveChanges();

                    SelectedRecipient = Recipients.FirstOrDefault();
                    return true;
                })
                { CanEdit = false, Title = "Удалить?" }
            };
            recipientWindow.ShowDialog();
            OnPropertyChanged("CountRecipients");
        }));
        #endregion

        private ICommand saveMessageDataCommand;
        public ICommand SaveMessageDataCommand => saveMessageDataCommand ?? (saveMessageDataCommand = new RelayCommand((object par) => {
            var message = SelectedMessage ?? new Message();
            message.IsBodyHtml = true;
            message.Sender = message.Sender ?? SelectedSender;
            message.SenderId = message.Sender?.Id ?? 0;
            if (message.SenderId <= 0)
            {
                App.ShowDialogWarn("Выберите отправителя!");
                return;
            }
            message.Recipient = message.Recipient ?? SelectedRecipient;
            message.RecipientId = message.Recipient?.Id ?? 0;
            if (message.RecipientId <= 0)
            {
                App.ShowDialogWarn("Выберите получателя!");
                return;
            }
            message.Name = $"{message.Recipient.Address}: {message.Subject}";
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

            var upd = _mailsAndSendersDb.Messages.FirstOrDefault(x => x.Id == message.Id);
            if (upd == null)
            {
                _mailsAndSendersDb.Messages.Add(message);
                _mailsAndSendersDb.SaveChanges();

                Messages.Add(message);
                return;
            }

            upd.Name = message.Name;
            upd.Subject = message.Subject;
            upd.Body = message.Body;
            upd.IsBodyHtml = message.IsBodyHtml;
            upd.RecipientId = message.RecipientId;
            upd.SenderId = message.SenderId;

            _mailsAndSendersDb.SaveChanges();

            var index = Messages.IndexOf(Messages.FirstOrDefault(x => x.Id == message.Id));
            Messages.RemoveAt(index);
            Messages.Insert(index, message);
            SelectedMessage = message;
        }));

        //private ICommand _saveMessageCommand;
        //public ICommand SaveMessageCommand => _saveMessageCommand ?? (_saveMessageCommand = new RelayCommand((object par) => {
        //    App.ShowDialogInfo("Нажата кнопка сохранить сообщение");
        //}));

        private void AddTemplateTask()
        {
            /*для demo */
            if (SelectedSender == null || SelectedSender.Id <= 0)
            {
                App.ShowDialogWarn("Выберите отправителя!");
                return;
            }

            if (SelectedRecipient == null || SelectedRecipient.Id <= 0)
            {
                App.ShowDialogWarn("Выберите получателя!");
                return;
            }

            if (SelectedServer == null || SelectedServer.Id <= 0)
            {
                App.ShowDialogWarn("Выберите сервер!");
                return;
            }

            var count = SenderTasks.Count + 1;
            var date = SelectedCalendarDate ?? DateTime.Now;
            var newTask = new SenderTask
            {
                TaskDate = date.AddDays(-1),
                SendDate = date,
                Message = new Message
                {
                    Subject = $"Тема сообщения № {count} от {date:dd.MM.yyyy hh:mm:ss}",
                    Body = "Тело сообщения",
                    RecipientId = SelectedRecipient.Id,
                    Recipient = SelectedRecipient,
                    SenderId = SelectedSender.Id,
                    Sender = SelectedSender,
                    Name = $"{SelectedRecipient.Address}: Тема сообщения № {count} от {date:dd.MM.yyyy hh:mm:ss}"
                },
                Server = SelectedServer,
                ServerId = SelectedServer.Id,
                Name = $"Задание на отправку получателю {SelectedRecipient.Address}",
            };
            _mailsAndSendersDb.Messages.Add(newTask.Message);
            _mailsAndSendersDb.SaveChanges();

            Messages.Add(newTask.Message);

            newTask.MessageId = newTask.Message.Id;
            _mailsAndSendersDb.SenderTasks.Add(newTask);
            _mailsAndSendersDb.SaveChanges();
            SenderTasks.Add(newTask); 
            SelectedTask = SenderTasks.LastOrDefault();
        }
                
        private ICommand addNewTaskCommand;
        public ICommand AddNewTaskCommand => addNewTaskCommand ?? (addNewTaskCommand = new RelayCommand((object par) => {
            AddTemplateTask();
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
                    SenderTasks.RemoveAt(indx);
                    SenderTasks.Insert(indx, s);

                    s.Message.Name = $"{ s.Message.Recipient.Address}: { s.Message.Subject}";

                    var updMessage = _mailsAndSendersDb.Messages.FirstOrDefault(x => x.Id == s.MessageId);
                    if (updMessage == null)
                    {
                        _mailsAndSendersDb.Messages.Add(s.Message);
                        _mailsAndSendersDb.SaveChanges();

                        Messages.Add(updMessage);
                    }
                    else
                    {
                        updMessage.Name = s.Message.Name;
                        updMessage.Subject = s.Message.Subject;
                        updMessage.Body = s.Message.Body;
                        updMessage.IsBodyHtml = s.Message.IsBodyHtml;
                        updMessage.RecipientId = s.Message.RecipientId;
                        updMessage.SenderId = s.Message.SenderId;
                        _mailsAndSendersDb.SaveChanges();
                    }


                    var upd = _mailsAndSendersDb.SenderTasks.FirstOrDefault(x => x.Id == s.Id);
                    upd.ServerId = s.ServerId;
                    upd.MessageId = updMessage.Id;
                    upd.Name = $"Задание на отправку получателю {s.Message.Recipient.Address}";
                    upd.SendDate = s.SendDate;
                    _mailsAndSendersDb.SaveChanges();

                    SelectedTask = s;
                    return true;
                })
                {
                    Recipients = this.Recipients,
                    Senders = this.Senders,
                    Servers = this.Servers,
                    CanEditSendDate = false
                }
            };
            senderWindow.ShowDialog();
        }));

        private ICommand removeTaskCommand;
        public ICommand RemoveTaskCommand => removeTaskCommand ?? (removeTaskCommand = new RelayCommand((object par) => {

            var delMessage = _mailsAndSendersDb.Messages.FirstOrDefault(x => x.Id == SelectedTask.MessageId);
            if (delMessage != null)
            {
                _mailsAndSendersDb.Messages.Remove(delMessage);
                Messages.Remove(Messages.FirstOrDefault(x => x.Id == SelectedTask.MessageId));
                SelectedMessage = Messages.FirstOrDefault();
            }

            var delTask = _mailsAndSendersDb.SenderTasks.FirstOrDefault(x => x.Id == SelectedTask.Id);
            _mailsAndSendersDb.Remove(delTask);
            _mailsAndSendersDb.SaveChanges();
                        
            SenderTasks.Remove(SelectedTask);
            SelectedTask = SenderTasks.LastOrDefault();
        }));

        private ICommand undoTaskCommand;
        public ICommand UndoTaskCommand => undoTaskCommand ?? (undoTaskCommand = new RelayCommand((object par) => {

            var messageIds = SenderTasks.Select(s => s.MessageId);
            _mailsAndSendersDb.Messages.RemoveRange(_mailsAndSendersDb.Messages.Where(x => messageIds.Contains(x.Id)));
            _mailsAndSendersDb.SenderTasks.RemoveRange(_mailsAndSendersDb.SenderTasks);
            _mailsAndSendersDb.SaveChanges();

            messageIds.ToList().ForEach(x => Messages.Remove(Messages.FirstOrDefault(y=> x == y.Id)));
            SenderTasks.Clear();
            SelectedMessage = Messages.FirstOrDefault();
        }));


        /*
         private ICommand _templateCommand;
         public ICommand TemplateCommand => _templateCommand ?? (_templateCommand = new RelayCommand((object par) => {
            //команда

         })); 
         */
    }
}
