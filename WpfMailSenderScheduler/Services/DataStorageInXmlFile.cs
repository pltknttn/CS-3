using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization; 
using WpfMailSenderLibrary.Models;
using WpfMailSenderLibrary.Interfaces;

namespace WpfMailSenderScheduler.Services
{
    public class DataStorageInXmlFile : IServersStorage, IRecipientsStorage, ISendersStorage, IMessagesStorage
    {
        class XmlFileDataStructure
        {
            public ICollection<Server> Servers { get; set; } = new List<Server>();
            public ICollection<Recipient> Recipients { get; set; } = new List<Recipient>();
            public ICollection<Sender> Senders { get; set; } = new List<Sender>();
            public ICollection<Message> Messages { get; set; } = new List<Message>();
        }

        private readonly string _fileName;

        public DataStorageInXmlFile(string fileName) => _fileName = fileName; 
        public DataStorageInXmlFile() : this(App.DataFileName) { }

        private XmlFileDataStructure Data { get; set; } = new XmlFileDataStructure(); 

        ICollection<Recipient> IStorage<Recipient>.Items => Data.Recipients;

        ICollection<Sender> IStorage<Sender>.Items => Data.Senders;

        ICollection<Message> IStorage<Message>.Items => Data.Messages;

        ICollection<Server> IStorage<Server>.Items => Data.Servers;

        public void Load()
        {
            Debug.WriteLine("Вызвана процедура загрузки данных из xml файла");

            if (!File.Exists(_fileName)) {
                Debug.WriteLine($"Файл {_fileName} не существует");
                Data = new XmlFileDataStructure();
                return;
            }

            using (var file = File.OpenText(_fileName))
                if (file.BaseStream.Length == 0)
                {
                    Debug.WriteLine($"Файл {_fileName} пуст");
                    Data = new XmlFileDataStructure();
                    return;
                }

            Stream fStream = null;
            try
            {
                var xmlFormat = new XmlSerializer(typeof(XmlFileDataStructure));
                fStream = new FileStream(_fileName, FileMode.Open, FileAccess.Read);
                Data = (XmlFileDataStructure)xmlFormat.Deserialize(fStream);
            }
            catch(Exception ex) 
            {
                Debug.WriteLine($"Ошибка при загрузке файла {_fileName}: {ex.InnerException?.Message??ex.Message}");
                Data = new XmlFileDataStructure();
                return; 
            }
            finally
            {
                fStream?.Close();
            }

        }
        public void SaveChanges()
        {
            Debug.WriteLine("Вызвана процедура сохранения данных в xml файл");
            Stream fStream = null;
            try
            {
                using (var file = File.CreateText(_fileName)) { }
                var xmlFormat = new XmlSerializer(typeof(XmlFileDataStructure));
                fStream = new FileStream(_fileName, FileMode.Create, FileAccess.Write);
                xmlFormat.Serialize(fStream, this);
            }
            finally
            {
                fStream?.Close();
            }
        }
    }
}