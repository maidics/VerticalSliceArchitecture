using System.Net;
using Shouldly;
using VsaTemplate.Features.Examples;
using VsaTemplate.Tests.Infrastructure;

namespace VsaTemplate.Tests.FeatureTests.Examples.Queries;

public sealed class GetExampleByIdTests : TestBase
{
    [Test]
    public async Task ShouldReturnNotFoundIfNotExists()
    {
        var response = await TestApp.SendGetAsync(
            $"api/{ExampleEndpoints.Prefix}/{Guid.NewGuid()}"
        );

        var errorText = await response.Content.ReadAsStringAsync();

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
