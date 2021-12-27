using DoodleForms.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoodleForms.Infrastructure.Data.EntityConfigurations;

public class OptionConfiguration : IEntityTypeConfiguration<Option>
{
    public void Configure(EntityTypeBuilder<Option> builder)
    {
        builder.Property(o => o.Text).HasMaxLength(2048).IsRequired();
    }
}