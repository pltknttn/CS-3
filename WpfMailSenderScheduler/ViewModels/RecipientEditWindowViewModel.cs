﻿using EFMailsAndSendersDb.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfMailSenderScheduler.Commands;
using WpfMailSenderScheduler.Data.ValidationRules; 

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
        private string _address;
        public string Address { get => _address;
            set
            {
                Set(ref _address, value);
                RemoveError("Address");
                if (!UtilValidation.ValidateAddress(value, out var error)) AddError("Address", error);
            } } 

        public RecipientEditWindowViewModel() : this(null) { }
        public RecipientEditWindowViewModel(Recipient recipient, Predicate<Recipient> saveFunc = null)
        {
            _saveFunc = saveFunc;
            if (recipient != null)
            {
                Id = recipient.Id;
                Name = recipient.Name;
                Address = recipient.Address; 
                Title = $"Редактирование получателя {recipient.Name}";
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
                         
            RemoveError("Address");

            if (!UtilValidation.ValidateAddress(Address, out var error))
                AddError("Address", error);

            if (HasErrors)
            {
                App.ShowDialogError("Исправьте ошибки ввода!");
                DialogResult = null;
                return;
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                App.ShowDialogError("Укажите фио или название");
                DialogResult = null;
                return;
            }

            if (string.IsNullOrWhiteSpace(Address))
            {
                App.ShowDialogError("Укажите адрес");
                DialogResult = null;
                return;
            }
            
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
