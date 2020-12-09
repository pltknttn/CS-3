using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WpfMailSenderLibrary.Interfaces; 

namespace WpfMailSenderLibrary.Services
{
    public class SmtpMailService : IMailService
    {
        public IMailSender GetSender(string server, int port, bool isSsl, string login, string password)
        {
            return new SmtpMailSender(server, port, isSsl, login, password);
        }
    }

    public class SmtpMailSender : IMailSender
    {
        public string Address { get; set; }
        public int Port { get; set; }
        public bool IsSsl { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public SmtpMailSender(string address, int port, bool isSsl, string login, string password)
        {
            Address = address;
            Port = port;
            IsSsl = isSsl;
            Login = login;
            Password = password;
        }

        public string Send(string from, string recipient, string subject, string body, bool isBodyHtml)
        {
            try
            {
                using (var mailMessage = new MailMessage())
                { 
                    var client = new SmtpClient(Address, Port)
                    {
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(Login, Password),
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        EnableSsl = true,
                        Timeout = 50000
                    };

                    recipient.Split(new char[] { ',', ';', ' ' }).Select(x => new MailAddress(x)).ToList().ForEach(mailMessage.To.Add);
                    mailMessage.From = new MailAddress(from);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = isBodyHtml;

                    client.Send(mailMessage);
                } 
            }
            catch (Exception ex)
            {
                return ex.InnerException?.Message ?? ex.Message; 
            }
            return null;
        }
    }
}
