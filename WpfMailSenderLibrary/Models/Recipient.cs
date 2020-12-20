using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSenderLibrary.Models
{ 
    public class Recipient : ICloneable
    { 
        public int Id { get; set; } 
        public string Name { get; set; } 
        public string Address { get; set; }
        public string Description { get; set; }
        public string FullName { get { return $"{Name}:{Address}"; } }

        public object Clone()
        {
            return this.MemberwiseClone() as Recipient;
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}
