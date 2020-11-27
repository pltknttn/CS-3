using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data; 
using System.Windows.Markup;
using System.Xml;
using System.Xml.Serialization;
using WpfMailSender.Data;

namespace WpfMailSender
{
    public static class Utils
    {
        public static string Program = Assembly.GetExecutingAssembly().Location;
        public static string AppDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string MailClientsFileName = $"{AppDirectory}\\MailClients.xml";

        public static List<MailClient> GetMailClients()
        { 
            var list = Load<MailClient>(MailClientsFileName);
            if (list == null || list.Count == 0)
            {
                list = new List<MailClient>()
                {
                    new MailClient { Domain = "yandex.ru", OutputPort = 587, InputPort = 993, Timeout = 5000, Ssl = true },
                    new MailClient { Domain = "gmail.com", OutputPort = 587, InputPort = 993, Timeout = 5000, Ssl = true },
                };
            }
            return list;
        }
          
        public static bool Save<T>(this List<T> list, string filename)
        {
            Stream fStream = null;
            try
            {
                XmlSerializer xmlFormat = new XmlSerializer(typeof(List<T>));
                fStream = new FileStream(filename, FileMode.Create, FileAccess.Write);
                xmlFormat.Serialize(fStream, list);
            }
            finally
            {
                fStream?.Close();
            }

            return true;
        }

        public static bool Save<T>(this T item, string filename)
        {
            Stream fStream = null;
            try
            {
                XmlSerializer xmlFormat = new XmlSerializer(typeof(T));
                fStream = new FileStream(filename, FileMode.Create, FileAccess.Write);
                xmlFormat.Serialize(fStream, item);
            }
            finally
            {
                fStream?.Close();
            }

            return true;
        }

        public static List<T> Load<T>(string filename)
        {
            if (!File.Exists(filename)) return null;
            Stream fStream = null;
            try
            {
                XmlSerializer xmlFormat = new XmlSerializer(typeof(List<T>));
                fStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                return (List<T>)xmlFormat.Deserialize(fStream);
            }
            catch { return null; }
            finally
            {
                fStream?.Close();
            } 
        }

        public static List<T> Load<T>(this List<T> list, string filename)
        {
            if (!File.Exists(filename)) return list;
            Stream fStream = null;
            try
            {
                XmlSerializer xmlFormat = new XmlSerializer(typeof(List<T>));
                fStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                list = (List<T>)xmlFormat.Deserialize(fStream);
            }
            finally
            {
                fStream?.Close();
            }

            return list;
        }

        public static T Load<T>(this T item, string filename)
        {
            if (!File.Exists(filename)) return item;
            Stream fStream = null;
            try
            {
                XmlSerializer xmlFormat = new XmlSerializer(typeof(T));
                fStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                item = (T)xmlFormat.Deserialize(fStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                fStream?.Close();
            }

            return item;
        }

        public static bool ShowOpenFileDialog(string filter, string defaultFileName, out string filename)
        {
            filename = string.Empty;
            var isSuccessful = false;
            var dialog = new OpenFileDialog { Filter = filter, FileName = defaultFileName };
            if (dialog.ShowDialog() == true)
            {
                isSuccessful = true;
                filename = dialog.FileName;
            }
            return isSuccessful;
        }

        public static bool ShowSaveFileDialog(string filter, string defaultFileName, out string filename)
        {
            filename = string.Empty;
            var isSuccessful = false;
            var dialog = new SaveFileDialog { Filter = filter, FileName = defaultFileName };
            if (dialog.ShowDialog() == true)
            {
                isSuccessful = true;
                filename = dialog.FileName;
            }
            return isSuccessful;
        }
         
        public static void ShowWarning(string message)
        {
            new MessageWindow("Внимание!!!", message, false).Show();
        }
    }
}
