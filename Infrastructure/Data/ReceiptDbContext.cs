using Microsoft.EntityFrameworkCore;
using ReceiptReader.Domain;

namespace ReceiptReader.Infrastructure.Data
{
    public class ReceiptDbContext : DbContext
    {
        public ReceiptDbContext(DbContextOptions<ReceiptDbContext> options)
            : base(options)
        {

        }

        public DbSet<ReceiptInfo> Receipts { get; set; } = default!;
        public DbSet<AnalysisLog> AnalysisLogs { get; set; } = default!;
        public DbSet<ReceiptLineItem> ReceiptLineItem { get; set; } = default!;
        public DbSet<ReceiptTaxLine> ReceiptTaxLines { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnalysisLog>(e =>
            {
                e.HasKey(al => al.Id);

                e.HasOne(al => al.ReceiptInfo)
                    .WithMany()
                    .HasForeignKey(al => al.ReceiptInfoId)
                    .HasPrincipalKey(r => r.FileId)
                    .OnDelete(DeleteBehavior.SetNull);

                e.Property(al => al.FileHash)
                    .HasColumnName("FileHash")
                    .IsRequired()
                    .HasMaxLength(64);

                e.Property(al => al.AnalysisDate)
                    .HasColumnName("AnalysisDate")
                    .IsRequired();

                e.Property(al => al.Status)
                    .HasColumnName("Status")
                    .IsRequired()
                    .HasMaxLength(50);

                e.Property(al => al.FailureReason)
                .HasMaxLength(255);
            });

            modelBuilder.Entity<ReceiptInfo>(e =>
            {
                e.HasKey(r => r.FileId);

                e.Property(r => r.VendorName)
                    .HasColumnName("VendorName")
                    .IsRequired()
                    .HasMaxLength(200);

                e.Property(r => r.TotalAmount)
                    .HasColumnName("TotalAmount")
                    .IsRequired()
                    .HasPrecision(18, 2);

                e.Property(r => r.TransactionDate)
                    .HasColumnName("TransactionDate")
                    .IsRequired(false);

                e.Property(r => r.Currency)
                    .HasColumnName("Currency")
                    .IsRequired()
                    .HasMaxLength(3);

                e.Property(r => r.TaxAmount)
                    .HasColumnName("TaxAmount")
                    .IsRequired(false)
                    .HasPrecision(18, 2);

                e.HasMany(r => r.LineItems)
                    .WithOne(li => li.ReceiptInfo)
                    .HasForeignKey(li => li.ReceiptInfoId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(r => r.TaxLines)
                    .WithOne(tl => tl.ReceiptInfo)
                    .HasForeignKey(tl => tl.ReceiptInfoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ReceiptLineItem>(e =>
            {
                e.HasKey(li => li.Id);

                e.HasOne(li => li.ReceiptInfo)
                    .WithMany(r => r.LineItems)
                    .HasForeignKey(li => li.ReceiptInfoId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.Property(li => li.Name).HasMaxLength(100);
                e.Property(li => li.ProductCode).HasMaxLength(100);
            });

            modelBuilder.Entity<ReceiptTaxLine>(e =>
            {
                e.HasKey(tl => tl.Id);

                e.HasOne(tl => tl.ReceiptInfo)
                    .WithMany(r => r.TaxLines)
                    .HasForeignKey(li => li.ReceiptInfoId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.Property(tl => tl.Percentage).HasPrecision(18, 4);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
