using DoodleForms.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoodleForms.Infrastructure.Data.EntityConfigurations;

public class FormConfiguration : IEntityTypeConfiguration<Form>
{
    public void Configure(EntityTypeBuilder<Form> builder)
    {
        builder.Property(f => f.Title).HasMaxLength(512).IsRequired();
        builder.Property(f => f.Description).HasMaxLength(2048);
        builder.HasMany(f => f.Questions)
            .WithOne(q => q.Form)
            .OnDelete(DeleteBehavior.Cascade);
    }
}