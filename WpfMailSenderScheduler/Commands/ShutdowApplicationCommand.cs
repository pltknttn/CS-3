using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfMailSenderScheduler.Commands
{
    public class ShutdowApplicationCommand: Command
    {      
        protected void Execute()
        {
            Execute(null);
        }

        protected override void Execute(object parameter)
        {
            if (parameter is ShutdownMode shutdownMode)
                App.Current.ShutdownMode = shutdownMode;
            else if (parameter is int exitCode)
                App.Current.Shutdown(exitCode);
            else App.Current.Shutdown();
        }
    }
}
