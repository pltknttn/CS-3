using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfMailSenderScheduler.Commands
{
    public class CloseWindowCommand: Command
    {
        protected override void Execute(object parameter)
        {
            var window = (parameter as Window)
                ?? Application.Current.Windows.Cast<Window>().FirstOrDefault(w=>w?.IsFocused == true)
                ?? Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w?.IsActive == true);
            window?.Close();
        }
    }
}
