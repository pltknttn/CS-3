using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMailSenderLibrary.Models; 

namespace WpfMailSenderScheduler 
{    
    public class SqliteDataContext : DbContext
    {
        public SqliteDataContext() : base("SqliteConnection")
        {
        }
        public DbSet<Email> Emails { get; set; }
    }
}
