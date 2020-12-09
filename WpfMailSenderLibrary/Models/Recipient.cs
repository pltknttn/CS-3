using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSenderLibrary.Models
{
    [Table(Name = "Recipient")]
    public class Recipient
    {
        [Column]
        public int Id { get; set; }
        [Column]
        public string Name { get; set; }
        [Column]
        public string Address { get; set; }

        public string FullName { get { return $"{Name}:{Address}"; } }

        public override string ToString()
        {
            return FullName;
        }
    }
}
