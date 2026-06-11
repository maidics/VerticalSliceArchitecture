using Shouldly;
using VsaTemplate.Features.Examples;
using VsaTemplate.Features.Examples.Queries;
using VsaTemplate.Tests.Infrastructure;
using VsaTemplate.Tests.Infrastructure.Common;

namespace VsaTemplate.Tests.FeatureTests.Examples.Queries;

public sealed class GetExamplesTests : FeatureTestBase
{
    [TestCase(0)]
    [TestCase(10)]
    public async Task ShouldReturnExamples(int exampleAmount)
    {
        List<Example> examples = [];

        for (var i = 0; i < exampleAmount; i++)
        {
            var example = new Example { Content = $"test{i}" };

            await TestApp.AddAsync(example);

            examples.Add(example);
        }

        var entityIds = examples.Select(x => x.Id).ToList();

        var handler = GetRequestHandler<GetExamplesQueryRequestHandler>();

        var dtos = await handler.Handle(CancellationToken.None);
        var dtoIds = dtos.Select(x => x.Id).ToList();

        entityIds.ShouldBeEquivalentTo(dtoIds);
    }
}
