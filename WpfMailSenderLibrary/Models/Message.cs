using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSenderLibrary.Models
{
    [Table(Name = "Message")]
    public class Message
    {
        [Column]
        public int Id { get; set; }
        [Column]
        public string Subject { get; set; }
        [Column]
        public string Body { get; set; }

        public override string ToString()
        {
            return $"{Id}: {Subject}";
        }
    }
}
