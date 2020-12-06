using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using HTMLConverter;
using WpfMailSenderScheduler.Converters;
using Xceed.Wpf.Toolkit;

namespace WpfMailSenderScheduler.Formatters
{
    /// <summary>
    /// Formats the RichTextBox text as Html
    /// </summary>
    public class HtmlFormatter : ITextFormatter
    {
        public string GetText(System.Windows.Documents.FlowDocument document)
        {
            var range = new TextRange(document.ContentStart, document.ContentEnd);
            var bodyXaml = string.Empty;
            using (MemoryStream stream = new MemoryStream())
            {
                range.Save(stream, DataFormats.Rtf);
                stream.Position = 0;

                using (StreamReader r = new StreamReader(stream))
                {
                    bodyXaml = r.ReadToEnd();
                    r.Close();
                }
                stream.Close();
            }
            var htmlText = RtfToHtmlConverter.ConvertRtfToHtml(bodyXaml);
            return htmlText;

            //TextRange tr = new TextRange(document.ContentStart, document.ContentEnd);
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    tr.Save(ms, DataFormats.Xaml);

            //    var txt = HtmlFromXamlConverter.ConvertXamlToHtml(UTF8Encoding.Default.GetString(ms.ToArray()));
            //    return txt;
            //}
        }

        public void SetText(System.Windows.Documents.FlowDocument document, string text)
        {
            text = HtmlToXamlConverter.ConvertHtmlToXaml(text, false);
            try
            {
                TextRange tr = new TextRange(document.ContentStart, document.ContentEnd);
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(text)))
                {
                    tr.Load(ms, DataFormats.Xaml);
                }
            }
            catch
            {
                throw new InvalidDataException("data provided is not in the correct Html format.");
            }
        }
    }
}
