using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VsaTemplate.Features.Example;

public sealed class ExampleConfiguration : IEntityTypeConfiguration<Example>
{
    public void Configure(EntityTypeBuilder<Example> builder)
    {
        builder.HasIndex(x => x.Content).IsUnique();
    }
}
