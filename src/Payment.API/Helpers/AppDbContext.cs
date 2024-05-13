using Microsoft.EntityFrameworkCore;
using Payment.API.Models;

namespace Payment.API.Helpers
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> context) : base(context)
        {

        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AccountTransaction> AccountTransactions { get; set; }
    }
}
