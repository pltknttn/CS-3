using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMailSenderLibrary.Interfaces;
using WpfMailSenderLibrary.Models;

namespace WpfMailSenderLibrary.Services
{
    public class DataStorageInMemory : IServersStorage, IRecipientsStorage, ISendersStorage, IMessagesStorage
    {
        public ICollection<Server> Servers { get; set; } = new List<Server>();
        public ICollection<Recipient> Recipients { get; set; } = new List<Recipient>();
        public ICollection<Sender> Senders { get; set; } = new List<Sender>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();

        ICollection<Recipient> IStorage<Recipient>.Items => Recipients;

        ICollection<Sender> IStorage<Sender>.Items => Senders;

        ICollection<Message> IStorage<Message>.Items => Messages;

        ICollection<Server> IStorage<Server>.Items => Servers;

        public void Load()
        {
            Debug.WriteLine("Вызвана процедура загрузки данных из памяти");
            Servers = new List<Server>
            {
                new Server { Id = 1, Address = "yandex.ru", Port = 587 },
                new Server { Id = 2, Address = "gmail.com", Port = 587 }
            };
            Recipients = new List<Recipient>
            {
                new Recipient{Id = 1, Name="Иванов", Address="ivanov@yandex.ru"}
            };
            Senders = new List<Sender>
            {
                new Sender{Id = 1, Address="yandex.ru", Login="test", Password="test", Name="Тестовый отправитель"}
            };
            Messages = Enumerable.Range(1, 20)
                .Select(i => new Message { Id = i, Subject = $"Сообщение № {i}, тестирование приложения Рассыльщик", Body = $"Тестовое сообщение № {i}" }).ToList();
        }
        public void SaveChanges()
        {
            Debug.WriteLine("Вызвана процедура сохранения данных в память");
            Debug.WriteLine("Данные в памяти, логика сохранения не требуется");
        }
    }
}
