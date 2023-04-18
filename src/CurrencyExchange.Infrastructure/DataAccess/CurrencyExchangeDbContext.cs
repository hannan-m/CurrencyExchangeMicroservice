using Microsoft.EntityFrameworkCore;
using CurrencyExchange.Core.Entities;

namespace CurrencyExchange.Infrastructure.DataAccess
{
    public class CurrencyExchangeDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<CurrencyExchangeTransaction> CurrencyExchangeTransactions { get; set; }

        public CurrencyExchangeDbContext(DbContextOptions<CurrencyExchangeDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply the entity mappings here
             modelBuilder.ApplyConfigurationsFromAssembly(typeof(CurrencyExchangeDbContext).Assembly);
        }
    }
}
