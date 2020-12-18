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
            // Эта настройка нужна для того, чтобы база данных автоматически
            // удалялась и заново создавалась при изменении структуры модели
            // (чтобы было удобно тестировать примеры)
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<MailsAndSendersDb>()); 
            Console.ReadLine();
        }
    }
}
