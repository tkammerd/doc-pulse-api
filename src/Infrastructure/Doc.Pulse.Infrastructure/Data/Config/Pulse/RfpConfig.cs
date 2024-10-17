using Doc.Pulse.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doc.Pulse.Infrastructure.Data.Config.Pulse;

internal class RfpConfig : IEntityTypeConfiguration<Rfp>
{
    public void Configure(EntityTypeBuilder<Rfp> builder)
    {
        builder.ToTable("Rfps", "Pulse")
            .HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .IsRequired()
            .HasColumnName("RfpId");

        builder.Property(e => e.Facility)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder.Property(e => e.FiscalYear)
            .IsRequired()
            .HasColumnType("smallint");

        builder.Property(e => e.RfpNumber)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder.Property(e => e.RfpDate); // Defaults from model

        builder.Property(e => e.Description)
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder.Property(e => e.ObjectCodeId)
            .IsRequired();
        builder.Property(e => e.VendorId)
            .IsRequired();
        builder.Property(e => e.AgencyId)
            .IsRequired();
        builder.Property(e => e.AccountOrganizationId)
            .IsRequired();
        builder.Property(e => e.ProgramId)
            .IsRequired();

        builder.Property(e => e.PurchaseOrderNumber)
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder.Property(e => e.AmountObligated)
            .HasColumnType("money");

        builder.Property(e => e.Completed)
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder.Property(e => e.CheckOrDocumentNumber)
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder.Property(e => e.Comments)
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder.Property(e => e.ReportingCategory)
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder.Property(e => e.VerifiedOnIsis)
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder.Property(e => e.RequestedBy)
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder.Property(p => p.RowVersion).IsRowVersion();

        builder.HasOne(p => p.ObjectCode)
            .WithMany(p => p.Rfps)
            .HasForeignKey(p => p.ObjectCodeId);

        builder.HasOne(p => p.Vendor)
            .WithMany(p => p.Rfps)
            .HasForeignKey(p => p.VendorId);

        builder.HasOne(p => p.Agency)
            .WithMany(p => p.Rfps)
            .HasForeignKey(p => p.AgencyId);

        builder.HasOne(p => p.AccountOrganization)
            .WithMany(p => p.Rfps)
            .HasForeignKey(p => p.AccountOrganizationId);

        builder.HasOne(p => p.Program)
            .WithMany(p => p.Rfps)
            .HasForeignKey(p => p.ProgramId);
    }
}