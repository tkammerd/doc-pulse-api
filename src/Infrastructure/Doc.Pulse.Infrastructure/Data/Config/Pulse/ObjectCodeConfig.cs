using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Doc.Pulse.Core.Entities;

namespace Doc.Pulse.Infrastructure.Data.Config.Pulse;

internal class ObjectCodeConfig : IEntityTypeConfiguration<ObjectCode>
{
    public void Configure(EntityTypeBuilder<ObjectCode> builder)
    {
        builder.ToTable("ObjectCodes", "Pulse")
            .HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .IsRequired()
            .HasColumnName("ObjectCodeId");

        builder.Property(e => e.CodeName)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder.HasOne(p => p.CodeCategory)
            .WithMany(p => p.ObjectCodes)
            .HasForeignKey(p => p.CodeCategoryId);
    }
}