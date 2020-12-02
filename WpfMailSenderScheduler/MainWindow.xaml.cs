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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfMailSenderScheduler.Data;

namespace WpfMailSenderScheduler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var list = new DBWorker().Emails.ToList();
            var list2 = new DBWorker().Servers.ToList();
            var l2 = new DBWorker().Senders.ToList();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GotoScheduler_Click(object sender, RoutedEventArgs e)
        {
            Tabs.SelectedItem = tabScheduler;
        }

        private void SelectSender_ButtonAddClick(object sender, RoutedEventArgs e)
        {

        }

        private void SelectSender_ButtonDelClick(object sender, RoutedEventArgs e)
        {

        }

        private void SelectSender_ButtonEditClick(object sender, RoutedEventArgs e)
        {

        }

        private void SelectServer_ButtonAddClick(object sender, RoutedEventArgs e)
        {

        }

        private void SelectServer_ButtonEditClick(object sender, RoutedEventArgs e)
        {

        }

        private void SelectServer_ButtonDelClick(object sender, RoutedEventArgs e)
        {

        }

        private void SaveLetter_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
