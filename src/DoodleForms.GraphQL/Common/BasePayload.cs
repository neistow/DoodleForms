using System.Collections.Generic;

namespace DoodleForms.GraphQL.Common;

public record PayloadError(string Message, string Code);

public class PayloadBase
{
    public PayloadBase(IReadOnlyList<PayloadError>? errors)
    {
        Errors = errors;
    }

    public PayloadBase(PayloadError error) : this(new[] {error})
    {
    }

    public IReadOnlyList<PayloadError>? Errors { get; }
}