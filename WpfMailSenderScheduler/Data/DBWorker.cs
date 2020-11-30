using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMailSenderScheduler.Models;
using WpfMailSenderScheduler;
using System.Data.Linq;

namespace WpfMailSenderScheduler.Data
{
    public class DBWorker
    {
        private EMailsDataContext emails = new EMailsDataContext();
        public IQueryable<Message> Messages
        {
            get
            {
                Table<Message> messages = emails.GetTable<Message>();
                return messages;
            }
        }
    }
}
