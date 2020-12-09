using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace WpfMailSenderScheduler.Converters
{
    public class RtfToPlainTextConverter
    {
        public static string ConvertRtfToPlainText(string rtfString)
        {
            var xamlTextBody = string.IsNullOrWhiteSpace(rtfString) ? null : HTMLConverter.HtmlToXamlConverter.ConvertHtmlToXaml(rtfString, false);
            return string.IsNullOrWhiteSpace(xamlTextBody) ? null : Converters.XamlToPlainTextConverter.ConvertRtfToXaml(xamlTextBody);
        } 
    }
}
