using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSenderLibrary.Models
{ 
    public class Message : ICloneable
    { 
        public int Id { get; set; }
      
        public string Name { get; set; }        
   
        public string Subject { get; set; }
  
        public string Body { get; set; }
        
        public bool IsBodyHtml { get; set; }

        public int SenderId { get; set; }

        public int RecipientId { get; set; } 

        public Sender Sender { get; set; }
        
        public Recipient Recipient { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone() as Message;
        }

        public override string ToString()
        {
            return $"{Id}: {Subject}";
        }
    }
}
