using System.Collections.Generic;
using DoodleForms.Domain.Models;
using DoodleForms.GraphQL.Common;

namespace DoodleForms.GraphQL.Forms;

public class FormPayloadBase : PayloadBase
{
    public FormPayloadBase(Form? form = null, IReadOnlyList<PayloadError>? errors = null) : base(errors)
    {
        Form = form;
    }

    public FormPayloadBase(PayloadError error) : base(error)
    {
    }

    public Form? Form { get; }
}