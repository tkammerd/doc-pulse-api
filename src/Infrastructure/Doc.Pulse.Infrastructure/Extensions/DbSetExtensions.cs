using Microsoft.EntityFrameworkCore;
using Doc.Pulse.Core.Abstractions;
using System.Diagnostics;

namespace Doc.Pulse.Infrastructure.Extensions;

public static class DbSetExtensions
{
    public static async Task<TEntity> GetOrAddAsync<TEntity, TIdentifier>(this DbSet<TEntity> dbSet, EntityBase<TIdentifier> untrackedEntity, CancellationToken cancellationToken = default)
        where TEntity : EntityBase<TIdentifier>
        where TIdentifier : class
    {
        if (untrackedEntity is null)
            throw new ArgumentNullException(nameof(untrackedEntity));

        if (untrackedEntity is TEntity entity)
        {
            var lookup = await dbSet.SingleOrDefaultAsync(o => o.Id == entity.Id, cancellationToken);

            if (lookup == null)
            {
                _ = await dbSet.AddAsync(entity, cancellationToken);
            }

            return lookup ?? entity;
        }

        throw new UnreachableException($"{nameof(GetOrAddAsync)} Something terrible has happened.", new InvalidCastException("Well this Should Not be the case - What did you do?"));
    }
}

