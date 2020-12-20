using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMailsAndSendersDb.Data
{
    public class Recipient : NamedEntity
    {
        [Column, Required, MinLength(6), MaxLength(100)]
        public string Address { get; set; }

        [Column, MaxLength(500)]
        public string Description { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
