using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WpfMailSender.Data;

namespace WpfMailSender
{
    public class EmailSendService
    {
        public bool SendMessage(Mail mail)
        {
            try
            {
                using (var mailMessage = new MailMessage())
                {
                    var from = $"{mail.Login}@{mail.Domain}";
                    var client = new SmtpClient($"smtp.{mail.Domain}", mail.Port)
                    {
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(from, mail.Password),
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        EnableSsl = mail.Ssl,
                        Timeout = 50000
                    };

                    mail.ToAddr.Split(new char[] { ',', ';', ' ' }).Select(x => new MailAddress(x)).ToList().ForEach(mailMessage.To.Add);
                    mailMessage.From = new MailAddress(from);
                    mailMessage.Subject = mail.Subj;
                    mailMessage.Body = mail.Body;
                    mailMessage.IsBodyHtml = mail.IsBodyHtml;

                    client.Send(mailMessage);
                }

                return true;
            }
            catch (Exception ex)
            {
                Utils.ShowWarning(ex.InnerException?.Message ?? ex.Message);
                return false;
            }
        }
    }

    public class Mail
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public int Port { get; set; }
        public bool Ssl { get; set; }
        public int Timeout { get; set; }
        public string ToAddr { get; set; }
        public string Subj { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
    }
}
