using Microsoft.EntityFrameworkCore;
using xBudget.Account.Api.Models.Database;

namespace xBudget.Account.Api.Database
{
    public class AccountDatabaseContext : DbContext
    {
        public DbSet<Models.Database.Account> Accounts { get; set; }

        public AccountDatabaseContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AccountMapping.Map(modelBuilder);
        }
    }
}
