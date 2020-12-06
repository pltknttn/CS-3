using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSenderScheduler.Models
{
    public class SenderTask
    { 
        public int Id { get; set; } 
        
        public DateTime TaskDate { get; set; }
        
        public DateTime SendDate { get; set; }
        
        public string Subject { get; set; } 
        
        public string Body { get; set; }
        
        public Sender Sender { get; set; }

        public string SenderName => Sender?.FullName;

        public Recipient Recipient { get; set; }

        public string RecipientName => Recipient?.FullName;


        public override string ToString()
        {
            return $"{TaskDate:dd.MM.yyyy hh:mm:ss}: {Recipient?.FullName}";
        }
    }
}
