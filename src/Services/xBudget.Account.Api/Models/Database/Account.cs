using Microsoft.EntityFrameworkCore;
using System;

namespace xBudget.Account.Api.Models.Database
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }

    public class AccountMapping
    {
        public static void Map(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable("Account");
            modelBuilder.Entity<Account>().HasKey(x => x.Id);
            modelBuilder.Entity<Account>().Property(x => x.Name).IsRequired().HasColumnType("varchar(100)");
            modelBuilder.Entity<Account>().Property(x => x.Description).IsRequired().HasColumnType("varchar(1024)");
            modelBuilder.Entity<Account>().Property(x => x.Active).IsRequired();
        }
    }
}
