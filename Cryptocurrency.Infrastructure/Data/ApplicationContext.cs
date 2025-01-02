using Cryptocurrency.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Cryptocurrency.Infrastructure.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<CryptoSymbol> CryptoSymbols { get; set; }
        public DbSet<SearchHistory> SearchHistories { get; set; }
        public DbSet<ExchangeRates> ExchangeRates { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CryptoSymbol>(entity =>
            {
                entity.HasKey(cs => cs.Id);
                entity.Property(cs => cs.Symbol)
                      .IsRequired()
                      .HasMaxLength(10);

                entity.HasMany(cs => cs.ExchangeRates)
                      .WithOne(er => er.CryptoSymbol)
                      .HasForeignKey(er => er.CryptoSymbolId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ExchangeRates>(entity =>
            {
                entity.HasKey(er => er.Id);
                entity.Property(er => er.Currency)
                      .IsRequired()
                      .HasMaxLength(5);
                entity.Property(er => er.Price).IsRequired();
                entity.Property(er => er.Timestamp).IsRequired();
            });

            modelBuilder.Entity<SearchHistory>()
                .HasOne(sh => sh.CryptoSymbol)
                .WithMany()
                .HasForeignKey(sh => sh.CryptoSymbolId);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(cs => cs.UserId);
            });
        }
    }
}
