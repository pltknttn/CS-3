using EFMailsAndSendersDb.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMailsAndSendersDb
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            using (var db = new MailsAndSendersDbModel(new DbContextOptionsBuilder<MailsAndSendersDbModel>().UseSqlServer(Utils.ConnectionString).Options))
            {
                 await db.Database.EnsureCreatedAsync();

                 var count = await db.Senders.CountAsync();

                 Console.WriteLine($"count={count}");
            }

            Console.ReadLine();
        }
    }
}
