using EF = EFMailsAndSendersDb.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMailSenderLibrary.Interfaces;
using WpfMailSenderLibrary.Models;

namespace WpfMailSenderLibrary.Services
{
    public class DataStorageInMsSqlDb : IServersStorage, IRecipientsStorage, ISendersStorage, IMessagesStorage, ISenderTasksStorage
    {
        public ICollection<Server> Servers { get; set; } = new List<Server>();
        public ICollection<Recipient> Recipients { get; set; } = new List<Recipient>();
        public ICollection<Sender> Senders { get; set; } = new List<Sender>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public ICollection<SenderTask> SenderTasks { get; set; } = new List<SenderTask>();

        ICollection<Recipient> IStorage<Recipient>.Items => Recipients;

        ICollection<Sender> IStorage<Sender>.Items => Senders;

        ICollection<Message> IStorage<Message>.Items => Messages;

        ICollection<Server> IStorage<Server>.Items => Servers;

        ICollection<SenderTask> IStorage<SenderTask>.Items => SenderTasks;

        private readonly string _connectionstring;

        public DataStorageInMsSqlDb(string connectionstring) => _connectionstring = connectionstring;
         
        public void Load()
        {
            using (var db = new EF.MailsAndSendersDbModel(new DbContextOptionsBuilder<EF.MailsAndSendersDbModel>().UseSqlServer(_connectionstring).Options))
            {
                db.Database.EnsureCreated();

                Senders = db.Senders.ToList().Select(s => new Sender
                {
                    Id = s.Id,
                    Address = s.Address,
                    Name = s.Name,
                    Description = s.Description
                }).ToList();

                Recipients = db.Recipients.Select(s => new Recipient
                {
                    Id = s.Id,
                    Address = s.Address,
                    Name = s.Name,
                    Description = s.Description
                }).ToList();

                Servers = db.Servers.Select(s => new Server
                {
                    Id = s.Id,
                    Address = s.Address,
                    Login = s.Login,
                    Password = s.Password,
                    Port = s.Port,
                    UseSSL = s.UseSSL
                }).ToList();

                Messages = db.Messages.Include(i => i.Sender).Include(i => i.Recipient).Select(s => new Message
                {
                    Id = s.Id,
                    Name = s.Name,
                    Subject = s.Subject,
                    Body = s.Body,
                    IsBodyHtml = s.IsBodyHtml,
                    RecipientId = s.RecipientId,
                    Recipient = new Recipient
                    {
                        Id = s.Recipient.Id,
                        Address = s.Recipient.Address,
                        Name = s.Recipient.Name,
                        Description = s.Recipient.Description
                    },
                    SenderId = s.SenderId,
                    Sender = new Sender
                    {
                        Id = s.Sender.Id,
                        Address = s.Sender.Address,
                        Name = s.Sender.Name,
                        Description = s.Sender.Description
                    },
                }).ToList();

                SenderTasks = (
                            from s in db.SenderTasks.Include(i => i.Server)
                            join m in db.Messages.Include(i => i.Sender).Include(i => i.Recipient) on s.MessageId equals m.Id into mes
                            from message in mes.DefaultIfEmpty()
                            select
                            new SenderTask
                            {
                                Id = s.Id,
                                Name = s.Name,
                                Attempt = s.Attempt,
                                MessageId = s.MessageId,
                                SendDate = s.SendDate,
                                TaskDate = s.TaskDate,
                                ErrorSend = s.Error,
                                IsSendEnd = s.IsSendEnd,
                                IsProcessed = s.IsProcessed,
                                IsSuccessful = s.IsSuccessful,
                                ServerId = s.ServerId,
                                Server = new Server
                                {
                                    Id = s.Server.Id,
                                    Address = s.Server.Address,
                                    Login = s.Server.Login,
                                    Password = s.Server.Password,
                                    Port = s.Server.Port,
                                    UseSSL = s.Server.UseSSL
                                },
                                Message = new Message
                                {
                                    Id = message.Id,
                                    Name = message.Name,
                                    Subject = message.Subject,
                                    Body = message.Body,
                                    IsBodyHtml = message.IsBodyHtml,
                                    RecipientId = message.RecipientId,
                                    Recipient = new Recipient
                                    {
                                        Id = message.Recipient.Id,
                                        Address = message.Recipient.Address,
                                        Name = message.Recipient.Name,
                                        Description = message.Recipient.Description
                                    },
                                    SenderId = message.SenderId,
                                    Sender = new Sender
                                    {
                                        Id = message.Sender.Id,
                                        Address = message.Sender.Address,
                                        Name = message.Sender.Name,
                                        Description = message.Sender.Description
                                    }
                                }
                            }).ToList();
            }
        }
                 
        public void SaveChanges()
        {
            if(this is IServersStorage)
            {

            }

            if (this is IRecipientsStorage recipientsStorage)
            {

            }
        } 
    }
}