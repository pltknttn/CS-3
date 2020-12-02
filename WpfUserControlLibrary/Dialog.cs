using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUserControlLibrary
{
    public static class Dialog
    {
        public static bool ShowOpenFile(string filter, string defaultFileName, out string filename)
        {
            filename = string.Empty;
            var isSuccessful = false;
            var dialog = new OpenFileDialog { Filter = filter, FileName = defaultFileName };
            if (dialog.ShowDialog() == true)
            {
                isSuccessful = true;
                filename = dialog.FileName;
            }
            return isSuccessful;
        }

        public static bool ShowSaveFile(string filter, string defaultFileName, out string filename)
        {
            filename = string.Empty;
            var isSuccessful = false;
            var dialog = new SaveFileDialog { Filter = filter, FileName = defaultFileName };
            if (dialog.ShowDialog() == true)
            {
                isSuccessful = true;
                filename = dialog.FileName;
            }
            return isSuccessful;
        }

        public static bool ShowInformation(string message)
        {
            return new MessageWindow("Информация!!!", message, false).ShowDialog() ?? true;
        }

        public static bool ShowInformation(string title, string message)
        {
            return new MessageWindow(title, message, false).ShowDialog() ?? true;
        }

        public static bool ShowWarning(string title, string message)
        {
            return new MessageWindow(title, message, true).ShowDialog() ?? true;
        }

        public static bool ShowWarning(string message)
        {
            return new MessageWindow("Внимание!!!", message, true).ShowDialog() ?? true;
        }

        public static bool ShowException(string message)
        {
            return new MessageWindow("Ошибка!!!", message, false).ShowDialog()??true;
        }

        public static bool ShowException(string title, string message)
        {
            return new MessageWindow(title, message, false).ShowDialog() ?? true;
        }
    }
}
