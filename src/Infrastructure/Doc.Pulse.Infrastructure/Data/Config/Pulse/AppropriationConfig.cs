using Doc.Pulse.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doc.Pulse.Infrastructure.Data.Config.Pulse;

internal class AppropriationConfig : IEntityTypeConfiguration<Appropriation>
{
    public void Configure(EntityTypeBuilder<Appropriation> builder)
    {
        builder.ToTable("Appropriations", "Pulse")
            .HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .IsRequired()
            .HasColumnName("AppropriationId");

        builder.Property(e => e.Facility)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder.Property(e => e.FiscalYear)
            .IsRequired()
            .HasColumnType("smallint");

        builder.Property(e => e.ProgramId)
            .IsRequired();

        builder.Property(e => e.ObjectCodeId)
            .IsRequired();

        builder.Property(e => e.CurrentModifiedAmount)
            .HasColumnType("money");

        builder.Property(e => e.PreEncumberedAmount)
            .HasColumnType("money");

        builder.Property(e => e.EncumberedAmount)
            .HasColumnType("money");

        builder.Property(e => e.ExpendedAmount)
            .HasColumnType("money");
        
        builder.Property(e => e.ProjectedAmount)
            .HasColumnType("money");
        
        builder.Property(e => e.PriorYearActualAmount)
            .HasColumnType("money");
        
        builder.Property(e => e.TotalObligated)
            .HasColumnType("money");

        builder.HasOne(p => p.Program)
            .WithMany(p => p.Appropriations)
            .HasForeignKey(p => p.ProgramId);

        builder.HasOne(p => p.ObjectCode)
            .WithMany(p => p.Appropriations)
            .HasForeignKey(p => p.ObjectCodeId);
    }
}