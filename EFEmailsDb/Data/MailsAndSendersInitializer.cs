using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFEmailsDb.Data
{
    // Эта настройка нужна для того, чтобы база данных автоматически
    // удалялась и заново создавалась при изменении структуры модели
    // (чтобы было удобно тестировать примеры)
    //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<MailsAndSendersDb>());
    //всегда удалять
    //Database.SetInitializer(new DropCreateDatabaseAlways<MailsAndSendersDb>());
    public class MailsAndSendersInitializer : DropCreateDatabaseIfModelChanges<MailsAndSendersDb> //DropCreateDatabaseAlways<MailsAndSendersDb>
    {
        protected override void Seed(MailsAndSendersDb context)
        {
            context.Senders.AddRange(Enumerable.Range(1, 10).Select(i => new Sender { Name = $"Sender {i}", Address = $"address_{i}@test.ru" }));
            context.Recipients.AddRange(Enumerable.Range(1, 10).Select(i => new Recipient { Name = $"Recipient {i}", Address = $"address_{i}@test.ru" }));
            base.Seed(context);
        }
    }
}
