using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMailSenderScheduler.Models;
using WpfMailSenderScheduler;
using System.Data.Linq;
using WpfMailSenderScheduler.Context;

namespace WpfMailSenderScheduler.Data
{
    public class DBWorker
    { 
        private SqliteDataContext sqliteDataContext = new SqliteDataContext();
        private EMailsDataContext mssqlDataContext = new EMailsDataContext(Properties.Settings.Default.MailsAndSendersConnectionString);

        public IQueryable<Email> Emails
        {
            get
            {
                Table<Email> mail = mssqlDataContext.GetTable<Email>();
                return mail;
            }
        }

        public IQueryable<Server> Servers
        {
            get
            {
                Table<Server> mail = mssqlDataContext.GetTable<Server>();
                return mail;
            }
        }

        public IQueryable<Sender> Senders
        {
            get
            {
                Table<Sender> mail = mssqlDataContext.GetTable<Sender>(); 
                return mail;
            }
        }

        public IQueryable<Recipient> Recipients
        {
            get
            {
                Table<Recipient> mail = mssqlDataContext.GetTable<Recipient>();
                return mail;
            }
        }

        public IQueryable<Message> Messages
        {
            get
            {
                Table<Message> mail = mssqlDataContext.GetTable<Message>();
                return mail;
            }
        }

        public bool AddEmail(Email email)
        {
            var row = new Context.Emails
            {
                Name = email.Name,
                Value = email.Value
            };
            mssqlDataContext.Emails.InsertOnSubmit(row);
            mssqlDataContext.SubmitChanges();
            return true;
        }

        public bool EditEmail(Email email)
        {
            var row = (from emails in mssqlDataContext.Emails
                         where emails.Id == email.Id
                        select emails).FirstOrDefault();
            if (row == null) return false;

            row.Name = email.Name;            
            row.Value = email.Value;

            mssqlDataContext.SubmitChanges();
            return true;
        }

        public bool DeleteEmail(Email email)
        {
            var row = (from emails in mssqlDataContext.Emails
                       where emails.Id == email.Id
                       select emails).FirstOrDefault();
            if (row == null) return false;
            mssqlDataContext.Emails.DeleteOnSubmit(row);
            mssqlDataContext.SubmitChanges();
            return true;
        }

        public bool AddServer(Server server)
        {
            var row = new Context.SmtpServers
            {
                Address = server.Address,
                Port = server.Port
            };
            mssqlDataContext.SmtpServers.InsertOnSubmit(row);
            mssqlDataContext.SubmitChanges();
            return true;
        }

        public bool EditServer(Server server)
        {
            var row = (from srv in mssqlDataContext.SmtpServers
                       where srv.Id == server.Id
                       select srv).FirstOrDefault();
            if (row == null) return false;

            row.Address = server.Address;
            row.Port = server.Port;

            mssqlDataContext.SubmitChanges();
            return true;
        }
        
        public bool DeleteServer(Server server)
        {
            var row = (from srv in mssqlDataContext.SmtpServers
                       where srv.Id == server.Id
                       select srv).FirstOrDefault();
            if (row == null) return false;
            mssqlDataContext.SmtpServers.DeleteOnSubmit(row);
            mssqlDataContext.SubmitChanges();
            return true;
        }
        
        public bool AddSender(Sender sender)
        {
            var row = new Context.Senders
            {
                Address = sender.Address,
                Name = sender.Name,
                Login = sender.Login,
                Password = sender.Password
            };
            mssqlDataContext.Senders.InsertOnSubmit(row);
            mssqlDataContext.SubmitChanges();
            return true;
        }

        public bool EditSender(Sender sender)
        {
            var row = (from sndr in mssqlDataContext.Senders
                       where sndr.Id == sender.Id
                       select sndr).FirstOrDefault();
            if (row == null) return false;

            row.Address = sender.Address;
            row.Name = sender.Name;
            row.Login = sender.Login;
            row.Password = sender.Password;

            mssqlDataContext.SubmitChanges();
            return true;
        }

        public bool DeleteSender(Sender sender)
        {
            var row = (from sndr in mssqlDataContext.Senders
                       where sndr.Id == sender.Id
                       select sndr).FirstOrDefault();
            if (row == null) return false;
            mssqlDataContext.Senders.DeleteOnSubmit(row);
            mssqlDataContext.SubmitChanges();
            return true;
        }

        public bool AddMessage(Message message)
        {
            var row = new Context.Messages
            {
                Subject = message.Subject,
                Body = message.Body, 
            };
            mssqlDataContext.Messages.InsertOnSubmit(row);
            mssqlDataContext.SubmitChanges();
            return true;
        }

        public bool EditMessage(Message message)
        {
            var row = (from sndr in mssqlDataContext.Messages
                       where sndr.Id == message.Id
                       select sndr).FirstOrDefault();
            if (row == null) return false;

            row.Subject = message.Subject;
            row.Body = message.Body; 

            mssqlDataContext.SubmitChanges();
            return true;
        }

        public bool DeleteMessage(Message message)
        {
            var row = (from sndr in mssqlDataContext.Messages
                       where sndr.Id == message.Id
                       select sndr).FirstOrDefault();
            if (row == null) return false;
            mssqlDataContext.Messages.DeleteOnSubmit(row);
            mssqlDataContext.SubmitChanges();
            return true;
        }

    }
}
