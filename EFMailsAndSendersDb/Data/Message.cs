using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMailsAndSendersDb.Data
{
    public class Message : NamedEntity
    {
        [Column, Required]
        [StringLength(255, MinimumLength = 5)]
        public string Subject { get; set; }

        [Column, StringLength(500, MinimumLength = 5)]
        public string Body { get; set; }

        [Column, Required]
        public bool IsBodyHtml { get; set; } 

        [Required, ForeignKey("Sender"), Column(TypeName = "int")]
        public int SenderId { get; set; }

        [Required, ForeignKey("Recipient"), Column(TypeName = "int")]
        public int RecipientId { get; set; } 

        public Sender Sender { get; set; }
        public Recipient Recipient { get; set; }  
        public ICollection<SenderTask> SenderTasks { get; set; }
    } 
}
