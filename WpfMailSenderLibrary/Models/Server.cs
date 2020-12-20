using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSenderLibrary.Models
{ 
    public class Server
    { 
        public int Id { get; set; } 
        public string Address { get; set; } 
        public int Port { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool UseSSL { get; set; }
        public string FullAddress { get { return $"{Address}:{Port}"; } }

        public override string ToString()
        {
            return FullAddress;
        }
    }
}
