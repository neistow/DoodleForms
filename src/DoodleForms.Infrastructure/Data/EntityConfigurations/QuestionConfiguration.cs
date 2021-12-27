using DoodleForms.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoodleForms.Infrastructure.Data.EntityConfigurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.Property(q => q.Text).HasMaxLength(8192);
        builder.HasMany(q => q.Options)
            .WithOne(o => o.Question)
            .OnDelete(DeleteBehavior.Cascade);
    }
}