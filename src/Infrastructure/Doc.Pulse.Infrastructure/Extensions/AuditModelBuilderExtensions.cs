using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Doc.Pulse.Core.Abstractions;
using Doc.Pulse.Core.Entities._Kernel;

namespace Doc.Pulse.Infrastructure.Extensions;

public static class AuditModelBuilderExtensions
{
    public static ModelBuilder ApplyDocSharedKernelConfigurations(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyAllConfigurationsFromCurrentAssembly();
        modelBuilder.ApplyShadowGenAuditBaseConfigurations().ApplyEntityAuditBaseConfigurations();
        return modelBuilder;
    }

    public static ModelBuilder ApplyShadowGenAuditBaseConfigurations(this ModelBuilder modelBuilder)
    {
        foreach (IMutableEntityType item in from o in modelBuilder.Model.GetEntityTypes()
                                            where o.ClrType.IsAssignableTo(typeof(ISqlGenTimestampBase))
                                            select o)
        {
            modelBuilder.Entity(item.ClrType).Property<DateTimeOffset>("SqlCreated").HasDefaultValueSql("SYSDATETIMEOFFSET()");
            modelBuilder.Entity(item.ClrType).Property<DateTimeOffset>("SqlModified").HasDefaultValueSql("SYSDATETIMEOFFSET()")
                .ValueGeneratedOnAdd();
            modelBuilder.Entity(item.ClrType).Property<string>("SqlModifiedUser").HasColumnType("varchar")
                .HasMaxLength(250)
                .HasDefaultValueSql("SYSTEM_USER")
                .ValueGeneratedOnAddOrUpdate();
        }
        return modelBuilder;
    }

    public static ModelBuilder ApplyEntityAuditBaseConfigurations(this ModelBuilder modelBuilder)
    {
        foreach (IMutableEntityType item in from o in modelBuilder.Model.GetEntityTypes()
                                            where o.ClrType.IsAssignableTo(typeof(IAuditableEntityBase))
                                            select o)
        {
            modelBuilder.Entity(item.ClrType).Property<DateTimeOffset>("Created").HasDefaultValueSql("SYSDATETIMEOFFSET()")
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity(item.ClrType).Property<DateTimeOffset>("Modified").HasDefaultValueSql("SYSDATETIMEOFFSET()");
            modelBuilder.Entity(item.ClrType).Property<int?>("CreatedUserId").IsRequired(required: false)
                .HasDefaultValueSql(null)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity(item.ClrType).HasOne(typeof(UserStub), "CreatedUser").WithMany()
                .HasForeignKey("CreatedUserId")
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity(item.ClrType).Property<int?>("ModifiedUserId").IsRequired(required: false)
                .HasDefaultValueSql(null);
            modelBuilder.Entity(item.ClrType).HasOne(typeof(UserStub), "ModifiedUser").WithMany()
                .HasForeignKey("ModifiedUserId")
                .OnDelete(DeleteBehavior.NoAction);
        }

        return modelBuilder;
    }
}

