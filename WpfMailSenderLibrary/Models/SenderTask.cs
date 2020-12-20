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
        
        public string Name { get; set; }

        public DateTime TaskDate { get; set; }
        
        public DateTime? SendDate { get; set; }

        public int ServerId { get; set; }

        public int Attempt { get; set; } 
         
        public int MessageId { get; set; }
         
        public string ErrorSend { get; set; }

        public bool IsSendEnd { get; set; } = false;

        public bool IsSuccessful { get; set; } = false;

        public bool IsProcessed { get; set; } = false;

        public Message Message { get; set; } 

        public Server Server { get; set; }

        public string RecipientName => Message?.Recipient?.FullName??string.Empty;

        public string SenderName => Message?.Sender?.FullName ?? string.Empty;

        public string Body
        {
            get => Message?.Body;
            set
            {
                Message = Message ?? new Message { Id = MessageId };
                Message.Body = value;
            }
        }

        public string Subject
        {
            get => Message?.Subject;
            set
            {
                Message = Message ?? new Message { Id = MessageId };
                Message.Subject = value;
            }
        }

        public Sender Sender
        {
            get => Message?.Sender;
            set
            {
                Message = Message ?? new Message { Id = MessageId };
                Message.Sender = value;
            }
        }

        public Recipient Recipient
        {
            get => Message?.Recipient;
            set
            {
                Message = Message ?? new Message { Id = MessageId };
                Message.Recipient = value;
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone() as SenderTask;
        }

        public override string ToString()
        {
            return $"{SendDate:dd.MM.yyyy hh:mm:ss}: {Name}";
        }
    }
}
