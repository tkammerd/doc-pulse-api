using Doc.Pulse.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doc.Pulse.Infrastructure.Data.Config.Pulse;

internal class AccountOrganizationConfig : IEntityTypeConfiguration<AccountOrganization>
{
    public void Configure(EntityTypeBuilder<AccountOrganization> builder)
    {
        builder.ToTable("AccountOrganizations", "Pulse")
            .HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .IsRequired()
            .HasColumnName("AccountOrganizationId");

        builder.Property(e => e.AccountOrganizationNumber)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder.Property(e => e.CostCenterDescription)
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder.Property(e => e.Inactive)
            .IsRequired();

        builder.Property(p => p.RowVersion).IsRowVersion();
    }
}