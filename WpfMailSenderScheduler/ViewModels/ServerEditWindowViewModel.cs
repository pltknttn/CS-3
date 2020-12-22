using EFMailsAndSendersDb.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfMailSenderScheduler.Commands;  

namespace WpfMailSenderScheduler.ViewModels
{
    public class ServerEditWindowViewModel : ViewModelBase
    {
        private bool _canEdit = true;
        public bool CanEdit { get => _canEdit; set => _canEdit = value; }

        private string _title = "Редактирование smtp-сервера";
        public string Title { get => _title; set => Set(ref _title, value); }

        
        public int Id { get; private set; } 
        public string Address { get; set; }
        public int Port { get; set; }
        public string Login { get; set; }
        public string Password { get; set; } 

        public ServerEditWindowViewModel() : this(null) { }
        public ServerEditWindowViewModel(Server server, Predicate<Server> saveFunc = null)
        {
            _saveFunc = saveFunc;
            if (server != null)
            {
                Id = server.Id; 
                Address = server.Address;
                Port = server.Port;
                Login = server.Login;
                Password = server.Password;
                Title = $"Редактирование smtp-сервера {server.Name}";
            }
        }

        private bool canClose = true;
        private bool? _dialogResult = null;
        public bool? DialogResult
        {
            get => _dialogResult;
            set => Set(ref _dialogResult, value);
        }

        private Predicate<Server> _saveFunc;
        private ICommand doOkCommand;
        public ICommand DoOkCommand => doOkCommand ?? (doOkCommand = new RelayCommand(() => {

            if (string.IsNullOrWhiteSpace(this.Address))
            {
                App.ShowDialogError("Укажите адрес");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.Login))
            {
                App.ShowDialogError("Укажите Login");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.Password))
            {
                App.ShowDialogError("Укажите пароль");
                return;
            }
            var server = new Server { Id = this.Id,  Address = this.Address, Port = this.Port, Login = this.Login, Password = this.Password, Name = $"smtp.{this.Login}:{this.Port}", UseSSL = true };
            if (_saveFunc?.Invoke(server) ?? false) DialogResult = true;
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
