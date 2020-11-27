using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSender.Data
{
    public sealed class MailClient
    {
        public string Domain { get; set; }
        public int OutputPort { get; set; }
        public int InputPort { get; set; }
        public bool Ssl { get; set; }
        public int Timeout { get; set; }

        public override string ToString()
        {
            return Domain;
        }
    }
}
