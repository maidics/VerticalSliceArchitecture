using Shouldly;
using VsaTemplate.Common.Models;
using VsaTemplate.Features.Examples;
using VsaTemplate.Features.Examples.Commands;
using VsaTemplate.Tests.Infrastructure;
using VsaTemplate.Tests.Infrastructure.Common;

namespace VsaTemplate.Tests.FeatureTests.Examples.Commands;

public sealed class UpdateExampleTests : FeatureTestBase
{
    [Test]
    public async Task ShouldReturnNotFoundIfExampleDoesNotExists()
    {
        var command = new UpdateExampleCommand(Guid.NewGuid(), "test");

        var handler = GetRequestHandler<UpdateExampleCommandHandler>();

        var result = await handler.Handle(command, CancellationToken.None);
        result.ShouldBeFailed(ResultType.NotFound, ["Example not found."]);
    }

    [Test]
    public async Task ShouldUpdateExampleContent()
    {
        var example = new Example { Content = "test" };

        await TestApp.AddAsync(example);

        var command = new UpdateExampleCommand(example.Id, "new-test-content");

        var handler = GetRequestHandler<UpdateExampleCommandHandler>();

        var result = await handler.Handle(command, CancellationToken.None);
        result.ShouldBeSuccessful();

        var updated = await TestApp.FirstOrDefaultAsync<Example>(x => x.Id == example.Id);
        updated!.Content.ShouldBe(command.Content);
    }
}
