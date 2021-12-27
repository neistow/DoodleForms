using System;
using DoodleForms.Domain.Abstract;

namespace DoodleForms.Domain.Models;

public class Option : IEntity<Guid>
{
    public string Text { get; set; } = default!;

    public Guid QuestionId { get; set; }
    public Question? Question { get; set; }
    public Guid Id { get; set; }
}