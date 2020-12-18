using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFEmailsDb.Data
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

        [Column, Required]
        [EnumDataType(typeof(MessageDirection))]
        public MessageDirection Direction { get; set; }

        [Required, ForeignKey("Sender"), Column(TypeName = "int")]
        public int SenderId { get; set; }

        [Column]
        public DateTime? SendDate { get; set; }

        [Column]
        public DateTime? ReceivedDate { get; set; }

        public Sender Sender { get; set; }
        public ICollection<MessageRecipient> MessageRecipients { get; set; }
        public ICollection<MessageAttachment> MessageAttachments { get; set; }
        public ICollection<TaskMessage> TaskMessages { get; set; }
    }


    public class MessageRecipient : Entity
    {
        [Required, ForeignKey("Message"), Column(TypeName = "int")]
        public int MessageId { get; set; }

        [Required, ForeignKey("Recipient"), Column(TypeName = "int")]
        public int RecipientId { get; set; }

        public Recipient Recipient { get; set; }
        public Message Message { get; set; }
    }

    public class MessageAttachment : Entity
    {
        [Required, ForeignKey("Message"), Column(TypeName = "int")]
        public int MessageId { get; set; }

        [Required]
        public byte[] Content { get; set; }

        public Message Message { get; set; }
    }

    public enum MessageDirection : int
    {
        Input = 1,
        Output = 2
    }
}
