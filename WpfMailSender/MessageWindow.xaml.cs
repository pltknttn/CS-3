using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfMailSender
{
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        public MessageWindow()
        {
            InitializeComponent();
            ButCancel.Visibility = Visibility.Collapsed;
        }

        public MessageWindow(string title, string message, bool showCancel):this()
        {
            ButCancel.Visibility = showCancel ? Visibility.Visible : Visibility.Collapsed;
            Title = title;
            Message.Text = message;
        }

        private void But_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
