using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMailsAndSendersDb.Data
{
    public class MailsAndSendersDbContextFactory : IDesignTimeDbContextFactory<MailsAndSendersDbModel>
    {
        public MailsAndSendersDbModel CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MailsAndSendersDbModel>();
            optionsBuilder.UseSqlServer(Utils.ConnectionString);

            return new MailsAndSendersDbModel(optionsBuilder.Options);
        }
    }
}
