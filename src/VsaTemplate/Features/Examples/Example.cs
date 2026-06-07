using VsaTemplate.Common.BaseClasses;

namespace VsaTemplate.Features.Examples;

public sealed class Example : BaseEntity
{
    public required string Content { get; set; }

    public void AppendContent(string additionalContent)
    {
        Content += additionalContent;
    }
}
