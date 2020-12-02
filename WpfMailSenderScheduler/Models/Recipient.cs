using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSenderScheduler.Models
{
    public class Recipient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public string FullName { get { return $"{Name}:{Address}"; } }

        public override string ToString()
        {
            return FullName;
        }
    }
}
