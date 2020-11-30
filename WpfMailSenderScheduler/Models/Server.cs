using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSenderScheduler.Models
{
    public class Server
    {
        public string Address { get; set; }
        public int Port { get; set; }

        public string FullAddress { get { return $"{Address}:{Port}"; } }
    }
}
