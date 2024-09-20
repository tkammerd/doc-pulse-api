using Doc.Pulse.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doc.Pulse.Infrastructure.Data.Config.Pulse;

internal class VendorConfig : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        builder.ToTable("Vendors", "Pulse")
            .HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .IsRequired()
            .HasColumnName("VendorId");

        builder.Property(e => e.VendorName)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder.Property(e => e.Inactive)
            .IsRequired();
    }
}