using Doc.Pulse.Core.Abstractions;
using Doc.Pulse.Core.Entities;
using Doc.Pulse.Core.Entities._Kernel;
using Doc.Pulse.Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace Doc.Pulse.Infrastructure.Data;

// add-migration Initial -o Data/Migrations -StartUpProject "Doc.Pulse.Api" -Context AppDbContext
// remove-migration -StartUpProject "Doc.Pulse.Api" -Context AppDbContext
// update-database -StartUpProject "Doc.Pulse.Api" -Context AppDbContext

public partial class AppDbContext : DbContext
{
    private readonly IMediator? _mediator;
    private readonly UserWithClaims _user;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        _user = UserWithClaims.NullUser;
    }

    public AppDbContext(DbContextOptions<AppDbContext> options, IMediator mediator, IResolveUserService resolveUserService)
        : base(options)
    {
        _mediator = mediator;
        _user = resolveUserService?.GetUserWithClaims() ?? UserWithClaims.NullUser;
    }

    // ///////////////////////////////////////////////////////////////
    // Define Entity DB Sets (in remainder of AppDbContext partial):
    // public virtual DbSet<CodeCategory> CodeCategories { get; set; }
    // public virtual DbSet<ObjectCode> ObjectCodes { get; set; }
    // public virtual DbSet<UserStub> UserStubs { get; set; }
    // ///////////////////////////////////////////////////////////////

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    // TAK_NOTE: Scaffolded databases add table configurations here.  It is preferred to have a separate config
    // TAK_NOTE: file for each table under Data\Config of the infrastructure project.
    {
        modelBuilder.HasDefaultSchema("Pulse");

        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.ApplyDocSharedKernelConfigurations();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        await this.UpdateAllShadowColumns(_user);

        
        int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);


        // ignore events if no dispatcher provided
        if (_mediator == null) return result;

        // dispatch events only if save was successful
        var entitiesWithEvents = ChangeTracker.Entries<EntityBase>()
            .Select(e => e.Entity)
            .Where(e => e.HasDomainEvents)
            .ToArray();

        foreach (var entity in entitiesWithEvents)
        {
            var events = entity.DomainEvents.ToArray();
            entity.ClearEvents();

            foreach (var domainEvent in events)
            {
                await _mediator.Publish(domainEvent, cancellationToken).ConfigureAwait(false);
            }
        }

        return result;
    }

    public override int SaveChanges()
    {
        return SaveChangesAsync().GetAwaiter().GetResult();
    }

}