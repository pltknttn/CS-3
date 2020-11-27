using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace WpfMailSender.Data
{
    public class UserManager
    {
        public MailClient MailClient { get; set; }
        public string Password { get; set; }
        public string Login { get; set; }
    }
}
