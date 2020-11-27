using System;
using System.Collections.Generic;
using System.IO;
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
using WpfMailSender.Converters;
using WpfMailSender.Data;

namespace WpfMailSender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();             
        }
         
        public UserManager User { get; set; } 

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title += $" {User.Login}@{User.MailClient.Domain}";
        }

        private string GetPlainBody()
        {
            try
            {
                var range = new TextRange(BodyEditor.Document.ContentStart, BodyEditor.Document.ContentEnd);
                var bodyTxt = string.Empty;
                using (MemoryStream stream = new MemoryStream())
                {
                    range.Save(stream, DataFormats.Text);
                    stream.Position = 0;

                    using (StreamReader r = new StreamReader(stream))
                    {
                        bodyTxt = r.ReadToEnd();
                        r.Close();
                    }
                    stream.Close();
                }
                return bodyTxt;
            }
            catch  
            {
                return "";
            }
        }

        private string GetBody()
        {
            try
            {
                var range = new TextRange(BodyEditor.Document.ContentStart, BodyEditor.Document.ContentEnd);
                var bodyXaml = string.Empty;
                using (MemoryStream stream = new MemoryStream())
                {
                    range.Save(stream, DataFormats.Rtf);
                    stream.Position = 0;

                    using (StreamReader r = new StreamReader(stream))
                    {
                        bodyXaml = r.ReadToEnd();
                        r.Close();
                    }
                    stream.Close();
                }
                return RtfToHtmlConverter.ConvertRtfToHtml(bodyXaml);
            }
            catch(Exception ex)
            {
                Utils.ShowWarning(ex.InnerException?.Message ?? ex.Message);
                return "";
            }
        }

        private void ButSend_Click(object sender, RoutedEventArgs e)
        {
            lSendEnd.Visibility = Visibility.Collapsed;

            var body = GetBody();
            if (string.IsNullOrWhiteSpace(body) || string.IsNullOrWhiteSpace(GetPlainBody()))
            {
                Utils.ShowWarning("Пустой текст сообщения");
                return;
            }
            if (string.IsNullOrWhiteSpace(ToAddr.Text))
            {
                Utils.ShowWarning("Не указано кому отправлять");
                return;
            }
            if (string.IsNullOrWhiteSpace(Subj.Text))
            {
                Utils.ShowWarning("Не указана тема");
                return;
            }
            var service = new EmailSendService();
            var mail = new Mail
            {
                Login = User.Login,
                Password = User.Password,
                Domain = User.MailClient.Domain,
                Port = User.MailClient.OutputPort,
                Ssl = User.MailClient.Ssl,
                Timeout = User.MailClient.Timeout,
                ToAddr = ToAddr.Text,
                Subj = Subj.Text,
                Body = body,
                IsBodyHtml = true
            };
            if(service.SendMessage(mail)) lSendEnd.Visibility = Visibility.Visible;
        }
    }
}
