using Microsoft.EntityFrameworkCore;
using NezihBankAPI.Models;

namespace NezihBankAPI.Data
{
    public class BankDbContext : DbContext
    {
        public DbSet<BankManagerAPI> BankManager { get; set; }
        public DbSet<PersonalCustomersAPI> PersonalCustomers { get; set; }
        public DbSet<PersonalAccountsAPI> Accounts { get; set; }
        public DbSet<PersonalTransactionsAPI> Transactions { get; set; }
        public DbSet<AnnouncementAPI> Announcements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=NezihBankDatabase;User Id=yonetici;Password=bjk-1903;TrustServerCertificate=true");
            base.OnConfiguring(optionsBuilder);
        }

    }
}
