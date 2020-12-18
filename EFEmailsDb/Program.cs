using EFEmailsDb.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFEmailsDb
{
    public class Program
    {
        public static void Main()
        {            
            using (var ctx = new MailsAndSendersDb())
            {
                var sender = new Sender() {Name = "DemoTest", Address="demo@test.eu"};
                ctx.Senders.Add(sender);
                ctx.SaveChanges();
            }
            Console.WriteLine("Demo completed.");
            Console.ReadLine(); 
        }
    }
}
