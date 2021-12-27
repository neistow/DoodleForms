using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DoodleForms.Domain.Models;
using DoodleForms.GraphQL.Options;
using DoodleForms.Infrastructure.Data;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace DoodleForms.GraphQL.Questions;

public class QuestionType : ObjectType<Question>
{
    protected override void Configure(IObjectTypeDescriptor<Question> descriptor)
    {
        descriptor.Field(q => q.Form).Ignore();
        descriptor.Field(q => q.FormId).Ignore();
        descriptor.Field(q => q.Options)
            .ResolveWith<QuestionResolvers>(r => r.GetOptions(default!, default!, default!, default!));
    }

    private class QuestionResolvers
    {
        [UseDbContext(typeof(ApplicationDbContext))]
        public async Task<IEnumerable<Option>> GetOptions(
            [Parent] Question question,
            [ScopedService] ApplicationDbContext dbContext,
            [Service] OptionDataLoader dataLoader,
            CancellationToken cancellationToken)
        {
            var optionIds = await dbContext.Questions
                .Where(q => q.Id == question.Id)
                .SelectMany(q => q.Options)
                .Select(o => o.Id)
                .ToListAsync(cancellationToken);

            return await dataLoader.LoadAsync(optionIds, cancellationToken);
        }
    }
}