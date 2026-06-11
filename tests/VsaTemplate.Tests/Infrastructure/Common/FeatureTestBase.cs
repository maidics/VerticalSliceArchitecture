using Microsoft.Extensions.DependencyInjection;
using VsaTemplate.Common.Interfaces;

namespace VsaTemplate.Tests.Infrastructure.Common;

public abstract class FeatureTestBase
{
    public IServiceScope Scope = null!;

    [SetUp]
    public async Task Setup()
    {
        await TestApp.ResetState();

        Scope = SetUp.ScopeFactory.CreateScope();
    }

    [TearDown]
    public void TearDown()
    {
        Scope.Dispose();
    }

    public TRequestHandler GetRequestHandler<TRequestHandler>()
        where TRequestHandler : IRequestHandler
    {
        return Scope.ServiceProvider.GetRequiredService<TRequestHandler>();
    }
}
