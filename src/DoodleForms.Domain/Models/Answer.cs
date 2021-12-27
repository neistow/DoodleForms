using System;

namespace DoodleForms.Domain.Models;

public class Answer
{
    public Guid QuestionId { get; set; }
    public Guid OptionId { get; set; }
    public Guid FormAnswerId { get; set; }
}