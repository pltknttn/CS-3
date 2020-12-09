using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfMailSenderScheduler.Commands;
using WpfMailSenderScheduler.Data;
using WpfMailSenderLibrary.Models;

namespace WpfMailSenderScheduler.ViewModels
{
    public class SenderEditWindowViewModel : ViewModelBase
    {
        private bool _canEdit = true;
        public bool CanEdit { get => _canEdit; set => _canEdit = value; }

        private string _title = "Редактирование отправителя";
        public string Title { get => _title; set => Set(ref _title, value); }

        private ObservableCollection<Server> _servers;
        public ObservableCollection<Server> Servers
        {
            get { return _servers; }
            set => Set(ref _servers, value);
        }

        public int Id { get; private set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }

        public SenderEditWindowViewModel() : this(null) { }
        public SenderEditWindowViewModel(Sender sender, Predicate<Sender> saveFunc = null)
        {
            _saveFunc = saveFunc;
            if (sender != null)
            {
                Id = sender.Id;
                Name = sender.Name;
                Address = sender.Address;
                Login = sender.Login;
                Pass = sender.Password;
                Title = $"Редактирование отправителя {sender.FullName}";
            }
        }

        private bool canClose = true;        
        private bool? _dialogResult = null;
        public bool? DialogResult {
            get => _dialogResult;
            set => Set(ref _dialogResult, value);
        }
        
        private Predicate<Sender> _saveFunc;
        private ICommand doOkCommand;
        public ICommand DoOkCommand => doOkCommand ?? (doOkCommand = new RelayCommand(()=> {

            var sender = new Sender { Id = this.Id, Name = this.Name, Address = this.Address, Login = this.Login, Password = Pass };
            if(_saveFunc?.Invoke(sender)??false) DialogResult = true;
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
