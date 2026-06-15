using VsaTemplate.Common.BaseClasses;
using VsaTemplate.Features.Examples.Events;

namespace VsaTemplate.Features.Examples;

public sealed class Example : BaseEntity
{
    public required string Content { get; set; }
    public bool HasAppendedContent { get; set; }

    public ExampleContentAppendedEvent AppendContent(string additionalContent)
    {
        Content += additionalContent;

        return new ExampleContentAppendedEvent(this);
    }
}
