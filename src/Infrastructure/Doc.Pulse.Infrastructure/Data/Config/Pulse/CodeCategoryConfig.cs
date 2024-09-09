using Doc.Pulse.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doc.Pulse.Infrastructure.Data.Config.Pulse;

internal class CodeCategoryConfig: IEntityTypeConfiguration<CodeCategory>
{
    public void Configure(EntityTypeBuilder<CodeCategory> builder)
    {
        builder.ToTable("CodeCategories", "Pulse")
            .HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .IsRequired()
            .HasColumnName("CodeCategoryId");

        builder.Property(e => e.CategoryShortName)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder.Property(e => e.CategoryName)
            .HasColumnType("varchar")
            .HasMaxLength(255);
    }
}