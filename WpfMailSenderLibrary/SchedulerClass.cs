using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WpfMailSenderLibrary.Interfaces;
using WpfMailSenderLibrary.Models;

namespace WpfMailSenderLibrary
{ 
    public class SchedulerClass
    {
        public void SendTask(List<SenderTask> senderTasks, IMailService service)
        {
            if (service == null) return;
                
            var dt = DateTime.Now.AddSeconds(-30);
            while(senderTasks.Exists(t=> !t.IsSendEnd && t.SendDate>=dt))
            {
                senderTasks.FindAll(t => !t.IsSendEnd && t.SendDate >= dt.AddSeconds(-30) && t.SendDate <= dt.AddSeconds(30))
                    .ForEach(s =>
                    {
                        s.IsSendEnd = true;
                        if (s.Server == null)
                        {
                            s.ErrorSend = "Не задан сервер отправки!";
                        }
                        else if (s.Sender == null)
                        {
                            s.ErrorSend = "Не задан отправитель!";
                        }
                        else if (s.Recipient == null)
                        {
                            s.ErrorSend = "Не задан получатель!";
                        }
                        else
                        {
                            var client = service.GetSender(s.Server.Address, s.Server.Port, true, s.Sender.Login, s.Sender.Password);
                            s.ErrorSend = client.Send(s.Sender.Email, s.Recipient.Address, s.Subject, s.Body, s.IsHtmlBody);
                        }
                       
                    });
                dt = DateTime.Now;
            }
        }

        public void SendTask(List<SenderTask> senderTasks)
        {
            var dt = DateTime.Now.AddSeconds(-30);
            while (senderTasks.Exists(t => t.SendDate >= dt))
            {
                senderTasks.FindAll(t => !t.IsSendEnd && t.SendDate >= dt.AddSeconds(-30) && t.SendDate <= dt.AddSeconds(30))
                    .ForEach(s =>
                    {   
                        s.IsSendEnd = true;
                        if (s.Server == null)
                        {
                            s.ErrorSend = "Не задан сервер отправки!";
                        }
                        else if (s.Sender == null)
                        {
                            s.ErrorSend = "Не задан отправитель!";
                        }
                        else if (s.Recipient == null)
                        {
                            s.ErrorSend = "Не задан получатель!";
                        }
                        else
                        {
                            s.ErrorSend = SendMessage(s.Sender.Login, s.Sender.Password, s.Server.Address, s.Server.Port, s.Recipient.Address, s.Subject, s.Body, s.IsHtmlBody);
                        }
                    });
                dt = DateTime.Now;
            }
        }

        private string SendMessage(string login, string password, string domain, int port, string toAddr, string subj, string body, bool htmlBody)
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
                    mailMessage.IsBodyHtml = htmlBody;

                    client.Send(mailMessage);
                }

                return null;
            }
            catch (Exception ex)
            {
                return ex.InnerException?.Message ?? ex.Message;
            }
        }
    }
}
