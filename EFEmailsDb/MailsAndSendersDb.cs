using EFEmailsDb.Data;
using System;
using System.Data.Entity;
using System.Linq;

namespace EFEmailsDb
{
    public class MailsAndSendersDb : DbContext
    {
        // Your context has been configured to use a 'MailsAndSendersDb' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'EFEmailsDb.MailsAndSendersDb' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'MailsAndSendersDb' 
        // connection string in the application configuration file.
        public MailsAndSendersDb()
            : base("name=MailsAndSendersDb")
        {
        }


        public virtual DbSet<Server> Servers { get; set; }
        public virtual DbSet<Sender> Senders { get; set; }
        public virtual DbSet<Recipient> Recipients { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<TaskMessage> TaskMessages { get; set; }
        public virtual DbSet<MessageRecipient> MessageRecipients { get; set; }
        public virtual DbSet<MessageAttachment> MessageAttachments { get; set; }

    }
}