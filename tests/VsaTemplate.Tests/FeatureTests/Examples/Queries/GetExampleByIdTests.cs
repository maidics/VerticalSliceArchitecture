using Shouldly;
using VsaTemplate.Common.Models;
using VsaTemplate.Features.Examples;
using VsaTemplate.Features.Examples.Queries;
using VsaTemplate.Tests.Infrastructure;
using VsaTemplate.Tests.Infrastructure.Common;

namespace VsaTemplate.Tests.FeatureTests.Examples.Queries;

public sealed class GetExampleByIdTests : FeatureTestBase
{
    [Test]
    public async Task ShouldReturnNotFoundIfExampleDoesNotExists()
    {
        var handler = GetRequestHandler<GetExampleByIdQueryRequestHandler>();

        var result = await handler.Handle(Guid.NewGuid(), CancellationToken.None);

        result.ShouldBeFailed(ResultType.NotFound, ["Example not found."]);
    }

    [Test]
    public async Task ShouldReturnSuccessIfExampleExists()
    {
        var example = new Example { Content = "test" };

        await TestApp.AddAsync(example);

        var handler = GetRequestHandler<GetExampleByIdQueryRequestHandler>();

        var result = await handler.Handle(example.Id, CancellationToken.None);

        result.ShouldBeSuccessful(ResultType.Success);
        result.Value.ShouldBeEquivalentTo(new ExampleDto(example.Id, example.Content));
    }
}
