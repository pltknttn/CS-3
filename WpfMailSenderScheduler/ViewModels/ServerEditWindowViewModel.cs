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
using WpfMailSenderScheduler.Models;

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

        public ServerEditWindowViewModel() : this(null) { }
        public ServerEditWindowViewModel(Server sender, Predicate<Server> saveFunc = null)
        {
            _saveFunc = saveFunc;
            if (sender != null)
            {
                Id = sender.Id; 
                Address = sender.Address;
                Port = sender.Port; 
                Title = $"Редактирование smtp-сервера {sender.FullAddress}";
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

            var server = new Server { Id = this.Id,  Address = this.Address, Port = this.Port };
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
