using System;
using System.Collections.Generic;
using DoodleForms.Domain.Abstract;

namespace DoodleForms.Domain.Models;

public class Form : IEntity<Guid>
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public bool AcceptAnswers { get; set; }
    public string CreatorId { get; set; } = default!;
    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public Guid Id { get; set; }
}