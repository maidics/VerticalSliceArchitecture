using Shouldly;
using VsaTemplate.Common.Models;
using VsaTemplate.Features.Examples;
using VsaTemplate.Features.Examples.Commands;
using VsaTemplate.Tests.Infrastructure;
using VsaTemplate.Tests.Infrastructure.Common;

namespace VsaTemplate.Tests.FeatureTests.Examples.Commands;

public sealed class DeleteExampleTests : FeatureTestBase
{
    [Test]
    public async Task ShouldReturnNotFoundIfExampleDoesNotExists()
    {
        var handler = GetService<DeleteExampleCommandRequestHandler>();

        var result = await handler.Handle(Guid.NewGuid().ToString(), CancellationToken.None);
        result.ShouldBeFailed(ResultType.NotFound, ["Example not found."]);
    }

    [Test]
    public async Task ShouldDeleteExample()
    {
        var example = new Example { Content = "test" };

        await TestApp.AddAsync(example);

        var handler = GetService<DeleteExampleCommandRequestHandler>();

        var result = await handler.Handle(example.Id, CancellationToken.None);
        result.ShouldBeSuccessful(ResultType.Success);

        var deleted = await TestApp.FirstOrDefaultAsync<Example>(x => x.Id == example.Id);
        deleted.ShouldBeNull();
    }
}
