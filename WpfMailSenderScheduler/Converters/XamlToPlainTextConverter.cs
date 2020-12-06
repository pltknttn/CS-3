using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WpfMailSenderScheduler.Converters
{
    public static class XamlToPlainTextConverter
    {
        public static string ConvertRtfToXaml(string xamlText)
        {
            var richTextBox = new RichTextBox();
            if (string.IsNullOrEmpty(xamlText)) return "";

            var textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
             
            using (var rtfMemoryStream = new MemoryStream())
            {
                using (var rtfStreamWriter = new StreamWriter(rtfMemoryStream))
                {
                    rtfStreamWriter.Write(xamlText);
                    rtfStreamWriter.Flush();
                    rtfMemoryStream.Seek(0, SeekOrigin.Begin);                     
                    textRange.Load(rtfMemoryStream, DataFormats.Xaml);
                }
            }
            using (var rtfMemoryStream = new MemoryStream())
            {

                textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                textRange.Save(rtfMemoryStream, DataFormats.Text);
                rtfMemoryStream.Seek(0, SeekOrigin.Begin);
                using (var rtfStreamReader = new StreamReader(rtfMemoryStream))
                {
                    return rtfStreamReader.ReadToEnd();
                }
            }
        }
    }
}
