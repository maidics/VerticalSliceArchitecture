using VsaTemplate.Features.Examples.Queries;
using VsaTemplate.Tests.Infrastructure.Common;

namespace VsaTemplate.Tests.FeatureTests.Examples.Queries;

public sealed class GetExampleByIdFeatureTests : FeatureTestBase
{
    [Test]
    public async Task ShouldReturnNotFoundIfNotExists()
    {
        var handler = GetRequestHandler<GetExampleByIdQueryRequestHandler>();

        var result = handler.Handle(Guid.NewGuid(), CancellationToken.None);
        
        result.
    }
}
