using System;
using System.Collections.Generic;

namespace DoodleForms.Domain.Models;

public class FormAnswer
{
    public Guid Id { get; set; }
    public Guid FormId { get; set; }
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
}