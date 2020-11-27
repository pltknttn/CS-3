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
using WpfMailSender.Data;

namespace WpfMailSender
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent(); 
        }   

        private void ButLogin_Click(object sender, RoutedEventArgs e)
        {
            var mailClient = cbMailClient.SelectedItem as MailClient;
            if (mailClient == null)
            {
                Utils.ShowWarning("Выберите почтовый домен");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.Login.Text))
            {
                Utils.ShowWarning("Укажите логин");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.Password.Password))
            {
                Utils.ShowWarning("Укажите пароль");
                return;
            }
            var userManager = new UserManager { MailClient = mailClient, Login = this.Login.Text, Password = this.Password.Password };

            this.Hide(); 
            App.Current.MainWindow = new MainWindow { User = userManager };
            App.Current.MainWindow.Show();
            this.Close();
        }

        private void ButCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
