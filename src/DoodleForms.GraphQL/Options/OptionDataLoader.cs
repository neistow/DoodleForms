using System;
using DoodleForms.Domain.Models;
using DoodleForms.GraphQL.Common;
using DoodleForms.Infrastructure.Data;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace DoodleForms.GraphQL.Options;

public class OptionDataLoader : BatchBaseDataLoader<Guid, Option>
{
    public OptionDataLoader(
        IDbContextFactory<ApplicationDbContext> dbContextFactory,
        IBatchScheduler batchScheduler,
        DataLoaderOptions? options = null)
        : base(dbContextFactory, batchScheduler, options)
    {
    }
}