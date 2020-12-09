using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSenderLibrary.Models
{
    public class SenderTask : ICloneable
    { 
        public int Id { get; set; } 
        
        public DateTime TaskDate { get; set; }
        
        public DateTime SendDate { get; set; }
        
        public string Subject { get; set; } 
        
        public string Body { get; set; }

        public bool IsHtmlBody { get; set; } = true;

        public Server Server { get; set; }

        public Sender Sender { get; set; }

        public string SenderName => Sender?.FullName;

        public Recipient Recipient { get; set; }

        public string RecipientName => Recipient?.FullName;

        public bool IsSendEnd { get; set; } 
        public string ErrorSend { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone() as SenderTask;
        }

        public override string ToString()
        {
            return $"{SendDate:dd.MM.yyyy hh:mm:ss}: {Recipient?.FullName}";
        }
    }
}
