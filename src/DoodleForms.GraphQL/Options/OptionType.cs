using DoodleForms.Domain.Models;
using HotChocolate.Types;

namespace DoodleForms.GraphQL.Options;

public class OptionType : ObjectType<Option>
{
    protected override void Configure(IObjectTypeDescriptor<Option> descriptor)
    {
        descriptor.Field(o => o.Question).Ignore();
        descriptor.Field(o => o.QuestionId).Ignore();
    }
}