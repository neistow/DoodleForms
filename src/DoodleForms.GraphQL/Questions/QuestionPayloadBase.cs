using System.Collections.Generic;
using DoodleForms.Domain.Models;
using DoodleForms.GraphQL.Common;

namespace DoodleForms.GraphQL.Questions;

public class QuestionPayloadBase : PayloadBase
{
    public QuestionPayloadBase(Question? question = null, IReadOnlyList<PayloadError>? errors = null) : base(errors)
    {
        Question = question;
    }

    public QuestionPayloadBase(PayloadError error) : base(error)
    {
    }

    public Question? Question { get; }
}