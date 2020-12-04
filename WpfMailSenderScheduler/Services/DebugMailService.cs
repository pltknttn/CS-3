using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMailSenderScheduler.Interfaces;

namespace WpfMailSenderScheduler.Services
{
    public class DebugMailService : IMailService
    {
        public IMailSender GetSender(string server, int port, bool isSsl, string login, string password)
        {
            return new DebugMailSender(server, port, isSsl, login, password);
        }
    }

    public class DebugMailSender : IMailSender
    {
        public string Address { get; set; }
        public int Port { get; set; }
        public bool IsSsl { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public DebugMailSender(string address, int port, bool isSsl, string login, string password)
        {
            Address = address;
            Port = port;
            IsSsl = isSsl;
            Login = login;
            Password = password;
        }

        public void Send(string from, string recipient, string subject, string body, bool isBodyHtml)
        {
            Debug.WriteLine($"Send from={from} to={recipient}");
            Debug.WriteLine($"Subject={subject}");
            Debug.WriteLine($"Body{(isBodyHtml?"HTML":"")}={body}");
        }
    }
}
