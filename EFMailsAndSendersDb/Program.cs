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
        static async Task Main(string[] args)
        {
            using (var db = new MailsAndSendersDbModel(new DbContextOptionsBuilder<MailsAndSendersDbModel>().UseSqlServer(Utils.ConnectionString).Options))
            {
                 await db.Database.EnsureCreatedAsync();

                 var senderscount = await db.Senders.CountAsync();
                 var recipientscount = await db.Recipients.CountAsync();
                 var serverscount = await db.Servers.CountAsync();
                 var messagescount = await db.Messages.CountAsync();
                 var taskscount = await db.SenderTasks.CountAsync();

                Console.WriteLine($"Servers count={serverscount}");
                Console.WriteLine($"Senders count={senderscount}");
                Console.WriteLine($"Recipients count={recipientscount}");
                Console.WriteLine($"Messages count={messagescount}");
                Console.WriteLine($"SenderTasks count={taskscount}");

                if (serverscount == 0)
                {
                    await db.Servers.AddRangeAsync( new List<Server> { 
                        new Server { Name = "smtp.yandex.ru", Address = "yandex.ru", Port = 587, Login = "test@yandex.ru", Password = "test" }
                    ,   new Server { Name = "smtp.mail.ru", Address = "mail.ru", Port = 465, Login = "geekbrains.test@inbox.ru", Password = "EtyOt3O3olC$" }
                    ,   new Server { Name = "smtp.gmail.com", Address = "gmail.com", Port = 587, Login = "test@gmail.com", Password = "test" }
                    });
                    db.SaveChanges();
                }

                if (recipientscount == 0)
                {
                    await db.Recipients.AddAsync(new Recipient { Name = "Полятыкина Татьяна", Address = "polyatikina.tatyana@gmail.com" });
                    db.SaveChanges();
                }

                if (senderscount == 0)
                {
                    await db.Senders.AddAsync(new Sender { Name = "Полятыкина Татьяна", Address = "polyatikina.tatyana@gmail.com" });
                    await db.Senders.AddAsync(new Sender { Name = "Tester", Address = "tester@gmail.com" });
                    await db.Senders.AddAsync(new Sender { Name = "Geek test", Address = "geekbrains.test@inbox.ru" });
                    db.SaveChanges();
                }
            }

            Console.ReadLine();
        }
    }
}
