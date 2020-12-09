using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSenderLibrary.Models
{
    [Table(Name = "Email")]
    public class Email
    {
        [Column]
        public int Id { get; set; }
        [Column]
        public string Name { get; set; }
        [Column]
        public string Value { get; set; }

        public override string ToString()
        {
            return $"{Id}: {Name}";
        }
    }
}
