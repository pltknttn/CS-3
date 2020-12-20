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
                            s.ErrorSend  = "Не задан сервер отправки!";
                        }
                        else if (s.Message == null)
                        {
                            s.ErrorSend = "Не задано сообщение!";
                        }
                        else if (s.Message.Sender == null)
                        {
                            s.ErrorSend  = "Не задан отправитель!";
                        }
                        else if (s.Message.Recipient == null)
                        {
                            s.ErrorSend = "Не задан получатель!";
                        }
                        else
                        {
                            var client = service.GetSender(s.Server.Address, s.Server.Port, true, s.Server.Login, s.Server.Password);
                            s.ErrorSend  = client.Send(s.Message.Sender.Address, s.Message.Recipient.Address, s.Message.Subject, s.Message.Body, s.Message.IsBodyHtml);
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
                        else if (s.Message == null)
                        {
                            s.ErrorSend = "Не задано сообщение!";
                        }
                        else if (s.Message.Sender == null)
                        {
                            s.ErrorSend = "Не задан отправитель!";
                        }
                        else if (s.Message.Recipient == null)
                        {
                            s.ErrorSend = "Не задан получатель!";
                        }
                        else
                        {
                            s.ErrorSend = SendMessage(s.Server.Login, s.Server.Password, s.Server.Address, s.Server.Port,
                                s.Message.Sender.Address, s.Message.Recipient.Address,
                                s.Message.Subject, s.Message.Body, s.Message.IsBodyHtml);
                        }
                    });
                dt = DateTime.Now;
            }
        }

        private string SendMessage(string login, string password, string domain, int port, string fromAddr, string toAddr, string subj, string body, bool htmlBody)
        {
            try
            {
                using (var mailMessage = new MailMessage())
                { 
                    var client = new SmtpClient($"smtp.{domain}", port)
                    {
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(login, password),
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        EnableSsl = true,
                        Timeout = 50000
                    };

                    toAddr.Split(new char[] { ',', ';', ' ' }).Select(x => new MailAddress(x)).ToList().ForEach(mailMessage.To.Add);
                    mailMessage.From = new MailAddress(fromAddr);
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
