using Doc.Pulse.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doc.Pulse.Infrastructure.Data.Config.Pulse;

internal class ProgramConfig : IEntityTypeConfiguration<Program>
{
    public void Configure(EntityTypeBuilder<Program> builder)
    {
        builder.ToTable("Programs", "Pulse")
            .HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .IsRequired()
            .HasColumnName("ProgramId");

        builder.Property(e => e.ProgramCode)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder.Property(e => e.ProgramName)
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder.Property(e => e.ProgramDescription)
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder.Property(e => e.Inactive)
            .IsRequired();

        builder.Property(p => p.RowVersion).IsRowVersion();
    }
}