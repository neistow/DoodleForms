using System;
using System.Collections.Generic;
using DoodleForms.Domain.Abstract;

namespace DoodleForms.Domain.Models;

public class Question : IEntity<Guid>
{
    public string? Text { get; set; }
    public bool Required { get; set; }
    public QuestionType QuestionType { get; set; }

    public Form? Form { get; set; }
    public Guid FormId { get; set; }

    public ICollection<Option> Options { get; set; } = new List<Option>();
    public Guid Id { get; set; }
}