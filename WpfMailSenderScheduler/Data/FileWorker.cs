using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WpfMailSenderScheduler.Models;

namespace WpfMailSenderScheduler.Data
{
    public class FileWorker
    { 
        public List<Server> Servers { get; set; } = new List<Server>();

        public List<Sender> Senders { get; set; } = new List<Sender>();

        public List<Recipient> Recipients { get; set; } = new List<Recipient>();

        public List<Message> Messages { get; set; } = new List<Message>();

        public static FileWorker LoadFromXml(string fileName)
        {
            if (!File.Exists(fileName)) return null;
            Stream fStream = null;
            try
            {
                var xmlFormat = new XmlSerializer(typeof(FileWorker));
                fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                return (FileWorker)xmlFormat.Deserialize(fStream);
            }
            catch { return null; }
            finally
            {
                fStream?.Close();
            }
        }

        public bool SaveToXml(string fileName)
        {
            Stream fStream = null;
            try
            {
                var xmlFormat = new XmlSerializer(typeof(FileWorker));
                fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                    xmlFormat.Serialize(fStream, this);
            }
            finally
            {
                fStream?.Close();
            }

            return true;
        }
    }
}
