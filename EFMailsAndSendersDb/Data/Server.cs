using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMailsAndSendersDb.Data
{
    public class Server : NamedEntity
    {
        [Column, Required, MinLength(6), MaxLength(100)]
        public string Address { get; set; }

        [Column, Required]
        public int Port { get; set; }

        [Column, Required]
        public bool UseSSL { get; set; }

        [Column, Required, StringLength(200)]
        public string Login { get; set; }

        [Column, Required, StringLength(100)]
        public string Password { get; set; } 

        public ICollection<SenderTask> SenderTasks { get; set; }
    } 
}
