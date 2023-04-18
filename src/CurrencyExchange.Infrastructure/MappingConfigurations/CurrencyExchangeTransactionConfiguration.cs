using CurrencyExchange.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Infrastructure.MappingConfigurations
{
    public class CurrencyExchangeTransactionConfiguration : IEntityTypeConfiguration<CurrencyExchangeTransaction>
    {
        public void Configure(EntityTypeBuilder<CurrencyExchangeTransaction> builder)
        {
            builder.ToTable("CurrencyExchangeTransactions");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();

            builder.Property(t => t.Timestamp).IsRequired();
            builder.Property(t => t.Amount).IsRequired().HasColumnType("decimal(18, 2)");
            builder.Property(t => t.Rate).IsRequired().HasColumnType("decimal(18, 4)");

            builder.Property(t => t.SourceCurrency).IsRequired();
            builder.Property(t => t.TargetCurrency).IsRequired();

            builder.HasOne(t => t.Client)
                .WithMany()
                .HasForeignKey(t => t.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
