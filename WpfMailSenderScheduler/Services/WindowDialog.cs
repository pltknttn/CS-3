using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMailSenderScheduler.Interfaces;
using WpfUserControlLibrary;

namespace WpfMailSenderScheduler.Services
{
    public class WindowDialog : IDialogService
    {
        public bool Show(string title, string msg) => Dialog.Show(title, msg); 
        public void ShowInfo(string msg) => Dialog.ShowInformation(msg);
        public void ShowInfo(string title, string msg) => Dialog.ShowInformation(title, msg);
        public void ShowWarn(string msg) => Dialog.ShowWarning(msg);
        public void ShowWarn(string title, string msg) => Dialog.ShowWarning(title, msg);
        public void ShowError(string msg) => Dialog.ShowException(msg);
        public void ShowError(string title, string msg) => Dialog.ShowException(title, msg);
        public void ShowQuestion(string msg) => Dialog.ShowQuestion(msg);
        public void ShowQuestion(string title, string msg) => Dialog.ShowQuestion(title, msg);
    }
}
