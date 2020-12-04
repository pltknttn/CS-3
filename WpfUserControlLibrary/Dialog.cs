using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        public static ImageSource ImageInfo => new BitmapImage(new Uri("pack://application:,,,/WpfUserControlLibrary;component/Images/information.png"));
        public static ImageSource ImageWarn => new BitmapImage(new Uri("pack://application:,,,/WpfUserControlLibrary;component/Images/warning.png"));
        public static ImageSource ImageQuestion => new BitmapImage(new Uri("pack://application:,,,/WpfUserControlLibrary;component/Images/ask_question.png"));
        public static ImageSource ImageError => new BitmapImage(new Uri("pack://application:,,,/WpfUserControlLibrary;component/Images/unavailable.png"));
        public static ImageSource ImageDefault => new BitmapImage(new Uri("pack://application:,,,/WpfUserControlLibrary;component/Images/chat_bubble.png"));

        public static bool ShowInformation(string message)
        {
            return ShowInformation("Информация!!!", message);
        }

        public static bool ShowInformation(string title, string message)
        {
            return new MessageWindow(title, message, false) { Icon = ImageInfo }.ShowDialog() ?? true;
        } 

        public static bool ShowWarning(string title, string message)
        {
            return new MessageWindow(title, message, true) { Icon = ImageWarn }.ShowDialog() ?? true;
        }
                
        public static bool ShowWarning(string message)
        {
            return ShowWarning("Внимание!!!", message);
        }

        public static bool ShowQuestion(string title, string message)
        {
            return new MessageWindow(title, message, true) { Icon = ImageQuestion }.ShowDialog() ?? true;
        }

        public static bool ShowQuestion(string message)
        {
            return ShowQuestion("Вопрос!!!", message);
        }

        public static bool ShowException(string message)
        {
            return ShowException("Ошибка!!!", message);
        }

        public static bool ShowException(string title, string message)
        {
            return new MessageWindow(title, message, false) { Icon = ImageError }.ShowDialog() ?? true;
        }

        public static bool Show(string title, string message)
        {
            return new MessageWindow(title, message, true) { Icon = ImageDefault }.ShowDialog() ?? true;
        }
    }
}
