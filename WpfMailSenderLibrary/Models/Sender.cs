using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSenderLibrary.Models
{
    [Table(Name = "Sender")]
    public class Sender
    {
        [Column]
        public int Id { get; set; }
        [Column]
        public string Name { get; set; }
        [Column]
        public string Address { get; set; }
        [Column]
        public string Login { get; set; }
        [Column]
        public string Password { get; set; }

        public string FullName { get { return $"{Name}:{Email}"; } }

        public string Email => $"{Login}@{Address}";

        public override string ToString()
        {
            return FullName;
        }
    }
}
