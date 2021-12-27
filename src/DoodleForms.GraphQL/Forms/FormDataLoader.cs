using System;
using DoodleForms.Domain.Models;
using DoodleForms.GraphQL.Common;
using DoodleForms.Infrastructure.Data;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace DoodleForms.GraphQL.Forms;

public class FormDataLoader : BatchBaseDataLoader<Guid, Form>
{
    public FormDataLoader(
        IDbContextFactory<ApplicationDbContext> dbContextFactory,
        IBatchScheduler batchScheduler,
        DataLoaderOptions? options = null)
        : base(dbContextFactory, batchScheduler, options)
    {
    }
}