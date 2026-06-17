using Shouldly;
using VsaTemplate.Features.Examples;

namespace VsaTemplate.Tests.FeatureTests.Examples;

public sealed class ExampleFeatureTests
{
    [Test]
    public void ShouldAppendContent()
    {
        string content = "content";
        string extra = "-extra-content";

        var example = new Example { Content = content };

        var @event = example.AppendContent(extra);

        example.Content.ShouldBe(content + extra);
        @event.ExampleId.ShouldBe(example.Id);
    }
}
