using Doc.Pulse.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doc.Pulse.Infrastructure.Data.Config.Pulse;

internal class ReceiptConfig : IEntityTypeConfiguration<Receipt>
{
    public void Configure(EntityTypeBuilder<Receipt> builder)
    {
        builder.ToTable("Receipts", "Pulse")
            .HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .IsRequired()
            .HasColumnName("ReceiptId");

        builder.Property(e => e.FiscalYear)
            .IsRequired()
            .HasColumnType("smallint");

        builder.Property(e => e.ReceiptNumber)
            .IsRequired();

        builder.Property(e => e.RfpId); // Defaults from model

        builder.Property(e => e.ReceiptDate); // Defaults from model

        builder.Property(e => e.ReceivingReportAmount)
            .HasColumnType("money");

        builder.Property(e => e.AmountInIsis)
            .HasColumnType("money");

        builder.Property(e => e.ReceiverNumber)
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder.Property(e => e.CheckNumber)
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder.Property(e => e.CheckDate); // Defaults from model

        builder.HasOne(p => p.Rfp)
            .WithMany(p => p.Receipts)
            .HasForeignKey(p => p.RfpId);
    }
}