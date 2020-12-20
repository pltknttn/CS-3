using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace EFMailsAndSendersDb.Data
{
    public class MailsAndSendersDbModel : DbContext
    {
        public MailsAndSendersDbModel() : base() { }
        public MailsAndSendersDbModel(DbContextOptions<MailsAndSendersDbModel> options) : base(options) { }

        public virtual DbSet<Server> Servers { get; set; }
        public virtual DbSet<Sender> Senders { get; set; }
        public virtual DbSet<Recipient> Recipients { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<SenderTask> SenderTasks { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            optionsBuilder.UseSqlServer(Utils.ConnectionString);
        }
    } 
}