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
using WpfMailSenderScheduler.Interfaces;
using WpfMailSenderScheduler.Models;
using WpfMailSenderScheduler.Views;
using Microsoft.Extensions.DependencyInjection;

namespace WpfMailSenderScheduler.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Title { get; set; } = "Рассыльщик почты";
        private ObservableCollection<Server> _servers;
        public ObservableCollection<Server> Servers
        {
            get { return _servers; }
            set => Set(ref _servers, value);
        }
        private ObservableCollection<Sender> _sender;
        public ObservableCollection<Sender> Senders
        {
            get { return _sender; }
            set => Set(ref _sender, value);
        }
        private ObservableCollection<Recipient> _recipients;
        public ObservableCollection<Recipient> Recipients
        {
            get { return _recipients; }
            set => Set(ref _recipients, value);
        }
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


        private ICommand loadDataCommand;
        public ICommand LoadDataCommand => loadDataCommand ?? (loadDataCommand = new RelayCommand((object par) =>
        {
            //var dbWorker = new DBWorker();
            //Servers = new ObservableCollection<Server>(dbWorker.Servers.ToList());
            //Senders = new ObservableCollection<Sender>(dbWorker.Senders.ToList());
            //Recipients = new ObservableCollection<Recipient>(dbWorker.Recipients.ToList());

            var fileWorker = FileWorker.LoadFromXml(App.DataFileName) ?? new FileWorker();
            Servers = new ObservableCollection<Server>(fileWorker.Servers);
            Senders = new ObservableCollection<Sender>(fileWorker.Senders);
            Recipients = new ObservableCollection<Recipient>(fileWorker.Recipients);
            Messages = new ObservableCollection<Message>(fileWorker.Messages);
        }));

        private ICommand saveDataCommand;
        public ICommand SaveDataCommand => saveDataCommand ?? (saveDataCommand = new RelayCommand((object par) => {
            var fileWorker = new FileWorker {
                Servers = this.Servers.ToList(),
                Senders = this.Senders.ToList(),
                Recipients = this.Recipients.ToList(),
                Messages = this.Messages.ToList()
            };
            fileWorker.SaveToXml(App.DataFileName);
        }));

        private ICommand createServerDataCommand;
        public ICommand CreateServerDataCommand => createServerDataCommand ?? (createServerDataCommand = new RelayCommand((object par) => {

            var serverWindow = new ServerEditWindow()
            {
                DataContext = new ServerEditWindowViewModel(null, (s) => {
                    s.Id = Servers.DefaultIfEmpty().Max(x => x?.Id ?? 0) + 1;
                    Servers.Add(s);
                    SelectedServer = s;
                    return true;
                })
            };
            serverWindow.ShowDialog();
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
                    SelectedServer = s;
                    return true;
                }) 
            };
            serverWindow.ShowDialog();
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
                    SelectedServer = Servers.FirstOrDefault();
                    return true;
                })
                { IsReadOnly = true } 
            };
            serverWindow.ShowDialog();
        }));

        private ICommand createSenderDataCommand;
        public ICommand CreateSenderDataCommand => createSenderDataCommand ?? (createSenderDataCommand = new RelayCommand((object par) => {
            var senderWindow = new SenderEditWindow()
            {
                DataContext = new SenderEditWindowViewModel(null, (s) => {
                    s.Id = Senders.DefaultIfEmpty().Max(x => x?.Id??0)  + 1;
                    Senders.Add(s);
                    SelectedSender = s;
                    return true;
                })
                {
                    Servers = this.Servers
                }
            };
            senderWindow.ShowDialog();
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
                    SelectedSender = s;
                    return true;
                })
                {
                    Servers = this.Servers
                }
            };
            senderWindow.ShowDialog();
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
                    SelectedSender = Senders.FirstOrDefault();
                    return true; })
                {
                    Servers = this.Servers,
                    IsReadOnly = true
                }
            };
            senderWindow.ShowDialog();
            
        }));


        private ICommand createRecipientDataCommand;
        public ICommand CreateRecipientDataCommand => createRecipientDataCommand ?? (createRecipientDataCommand = new RelayCommand((object par) => {

        }));

        private ICommand editRecipientDataCommand;
        public ICommand EditRecipientDataCommand => editRecipientDataCommand ?? (editRecipientDataCommand = new RelayCommand((object par) => {

        }));
        private ICommand deleteRecipientDataCommand;
        public ICommand DeleteRecipientDataCommand => deleteRecipientDataCommand ?? (deleteRecipientDataCommand = new RelayCommand((object par) => {

        }));
    }
}
