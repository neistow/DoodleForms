using System;
using DoodleForms.Domain.Models;
using DoodleForms.GraphQL.Common;
using DoodleForms.Infrastructure.Data;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace DoodleForms.GraphQL.Questions;

public class QuestionDataLoader : BatchBaseDataLoader<Guid, Question>
{
    public QuestionDataLoader(
        IDbContextFactory<ApplicationDbContext> dbContextFactory,
        IBatchScheduler batchScheduler,
        DataLoaderOptions? options = null)
        : base(dbContextFactory, batchScheduler, options)
    {
    }
}