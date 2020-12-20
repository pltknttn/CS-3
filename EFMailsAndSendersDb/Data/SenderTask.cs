using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMailsAndSendersDb.Data
{
    public class SenderTask : NamedEntity
    { 
        [Column, Required, ForeignKey("Server")]
        public int ServerId { get; set; }

        [Column, Required]
        public DateTime TaskDate { get; set; }

        [Column]
        public DateTime? SendDate { get; set; } 

        [Column, Required]
        public int Attempt { get; set; }
                
        [Column, Required, ForeignKey("Message")]
        public int MessageId { get; set; }

        [Column, Required]
        public bool IsProcessed { get; set; }

        [Column, Required]
        public bool IsSendEnd { get; set; }

        [Column, Required]
        public bool IsSuccessful { get; set; }

        [Column]
        public string Error { get; set; }

        public Server Server { get; set; }
        public Message Message { get; set; }
    } 
}
