using FluentValidation.TestHelper;
using Shouldly;
using VsaTemplate.Common.Models;
using VsaTemplate.Features.Examples;
using VsaTemplate.Features.Examples.Queries;
using VsaTemplate.FunctionalTests.Infrastructure;
using VsaTemplate.FunctionalTests.Infrastructure.Common;

namespace VsaTemplate.FunctionalTests.FeatureTests.Examples.Queries;

public sealed class GetExampleByIdTests : ApplicationTestBase
{
    [Test]
    public async Task ShouldReturnValidationErrors()
    {
        var query = new GetExampleByIdQuery("");
        var validator = GetService<GetExampleByIdQueryValidator>();

        var result = await validator.TestValidateAsync(query);
        result.IsValid.ShouldBeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public async Task ShouldNotReturnValidationErrors()
    {
        var query = new GetExampleByIdQuery("id");
        var validator = GetService<GetExampleByIdQueryValidator>();

        var result = await validator.TestValidateAsync(query);
        result.IsValid.ShouldBeTrue();
    }

    [Test]
    public async Task ShouldReturnNotFoundIfExampleDoesNotExists()
    {
        var query = new GetExampleByIdQuery("id");
        var handler = GetService<GetExampleByIdQueryHandler>();

        var result = await handler.Handle(query, CancellationToken.None);

        result.ShouldBeFailed(ResultType.NotFound, ["Example not found."]);
    }

    [Test]
    public async Task ShouldReturnSuccessIfExampleExists()
    {
        var example = new Example { Content = "test" };

        await Testing.AddAsync(example);

        var handler = GetService<GetExampleByIdQueryHandler>();

        var query = new GetExampleByIdQuery(example.Id);
        var result = await handler.Handle(query, CancellationToken.None);

        result.ShouldBeSuccessful();
        result.Value.ShouldBeEquivalentTo(new ExampleDto(example.Id, example.Content, false));
    }
}
