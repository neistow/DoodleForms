using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DoodleForms.Domain.Models;
using DoodleForms.GraphQL.Questions;
using DoodleForms.Infrastructure.Data;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace DoodleForms.GraphQL.Forms;

public class FormType : ObjectType<Form>
{
    protected override void Configure(IObjectTypeDescriptor<Form> descriptor)
    {
        descriptor.Field(f => f.CreatorId).Ignore();
        descriptor.Field(f => f.Questions)
            .ResolveWith<FormResolvers>(r => r.GetQuestions(default!, default!, default!, default!));
    }

    private class FormResolvers
    {
        [UseDbContext(typeof(ApplicationDbContext))]
        public async Task<IEnumerable<Question>> GetQuestions(
            [Parent] Form form,
            [ScopedService] ApplicationDbContext dbContext,
            [Service] QuestionDataLoader dataLoader,
            CancellationToken cancellationToken)
        {
            var questionIds = await dbContext.Questions
                .Where(q => q.FormId == form.Id)
                .Select(q => q.Id)
                .ToListAsync(cancellationToken);

            return await dataLoader.LoadAsync(questionIds, cancellationToken);
        }
    }
}