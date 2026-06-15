using Microsoft.Extensions.DependencyInjection;
using VsaTemplate.Common.Interfaces;
using VsaTemplate.Infrastructure;

namespace VsaTemplate.Tests.Infrastructure.Common;

public abstract class FeatureTestBase
{
    private IServiceScope Scope = null!;

    [SetUp]
    public async Task Setup()
    {
        await TestApp.ResetState();

        Scope = TestSetUpFixture.ScopeFactory.CreateScope();
    }

    [TearDown]
    public void TearDown()
    {
        Scope.Dispose();
    }

    protected TService GetService<TService>()
        where TService : notnull
    {
        return Scope.ServiceProvider.GetRequiredService<TService>();
    }
}
