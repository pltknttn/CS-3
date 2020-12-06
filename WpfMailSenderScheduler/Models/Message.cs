using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSenderScheduler.Models
{
   
    public class Message
    { 
        public int Id { get; set; } 
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; } 
        public string Body { get; set; }

        public override string ToString()
        {
            return $"{To}: {Subject}";
        }
    }
}
