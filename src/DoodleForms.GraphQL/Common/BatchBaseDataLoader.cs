using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DoodleForms.Domain.Abstract;
using DoodleForms.Infrastructure.Data;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace DoodleForms.GraphQL.Common;

public abstract class BatchBaseDataLoader<TKey, TEntity> : BatchDataLoader<TKey, TEntity>
    where TKey : notnull
    where TEntity : class, IEntity<TKey>
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    protected BatchBaseDataLoader(
        IDbContextFactory<ApplicationDbContext> dbContextFactory,
        IBatchScheduler batchScheduler,
        DataLoaderOptions? options = null) : base(
        batchScheduler, options)
    {
        _dbContextFactory = dbContextFactory;
    }

    protected override async Task<IReadOnlyDictionary<TKey, TEntity>> LoadBatchAsync(
        IReadOnlyList<TKey> keys,
        CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await context.Set<TEntity>()
            .Where(f => keys.Contains(f.Id))
            .ToDictionaryAsync(f => f.Id, cancellationToken);
    }
}