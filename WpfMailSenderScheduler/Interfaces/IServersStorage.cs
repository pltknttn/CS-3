using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMailSenderScheduler.Models;

namespace WpfMailSenderScheduler.Interfaces
{
    public interface IStorage<T>
    {
        ICollection<T> Items { get; }
        void Load();
        void SaveChanges();
    }

    public interface IServersStorage : IStorage<Server> { }
    public interface ISendersStorage : IStorage<Sender> { }
    public interface IRecipientsStorage : IStorage<Recipient> { }
    public interface IMessagesStorage : IStorage<Message> { }
}
