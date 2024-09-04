using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Doc.Pulse.Infrastructure.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyAllConfigurationsFromCurrentAssembly(this ModelBuilder modelBuilder, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();

        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
    }
}
