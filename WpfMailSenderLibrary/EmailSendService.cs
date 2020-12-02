using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSenderLibrary
{
    public class EmailSendService
    {
        public RunSendException SendException;

        public bool SendMessage(string login, string password, string domain, int port, string toAddr, string subj, string body)
        {
            try
            {
                using (var mailMessage = new MailMessage())
                {
                    var from = $"{login}@{domain}";
                    var client = new SmtpClient($"smtp.{domain}", port)
                    {
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(from, password),
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        EnableSsl = true,
                        Timeout = 50000
                    };

                    toAddr.Split(new char[] { ',', ';', ' ' }).Select(x => new MailAddress(x)).ToList().ForEach(mailMessage.To.Add);
                    mailMessage.From = new MailAddress(from);
                    mailMessage.Subject = subj;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;

                    client.Send(mailMessage);
                }

                return true;
            }
            catch (Exception ex)
            {
                SendException?.Invoke(ex.InnerException?.Message ?? ex.Message);
                return false;
            }
        }
    }

    public delegate void RunSendException(string error);
}
