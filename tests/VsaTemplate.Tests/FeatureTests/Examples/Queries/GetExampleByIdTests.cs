using Shouldly;
using VsaTemplate.Common.Models;
using VsaTemplate.Features.Examples;
using VsaTemplate.Features.Examples.Queries;
using VsaTemplate.Tests.Infrastructure;
using VsaTemplate.Tests.Infrastructure.Common;

namespace VsaTemplate.Tests.FeatureTests.Examples.Queries;

public sealed class GetExampleByIdTests : ApplicationTestBase
{
    [Test]
    public async Task ShouldReturnNotFoundIfExampleDoesNotExists()
    {
        var handler = GetService<GetExampleByIdQueryRequestHandler>();

        var result = await handler.Handle(Guid.NewGuid().ToString(), CancellationToken.None);

        result.ShouldBeFailed(ResultType.NotFound, ["Example not found."]);
    }

    [Test]
    public async Task ShouldReturnSuccessIfExampleExists()
    {
        var example = new Example { Content = "test" };

        await Testing.AddAsync(example);

        var handler = GetService<GetExampleByIdQueryRequestHandler>();

        var result = await handler.Handle(example.Id, CancellationToken.None);

        result.ShouldBeSuccessful(ResultType.Success);
        result.Value.ShouldBeEquivalentTo(new ExampleDto(example.Id, example.Content));
    }
}
