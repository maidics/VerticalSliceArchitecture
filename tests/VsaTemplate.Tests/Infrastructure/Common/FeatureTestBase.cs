using Microsoft.Extensions.DependencyInjection;
using VsaTemplate.Common.Interfaces;

namespace VsaTemplate.Tests.Infrastructure.Common;

public abstract class FeatureTestBase
{
#pragma warning disable NUnit1032
    public static IServiceScope Scope = SetUp.ScopeFactory.CreateScope();
#pragma warning restore NUnit1032
    public IServiceProvider ServiceCollection = Scope.ServiceProvider;

    [SetUp]
    public async Task Setup()
    {
        await TestApp.ResetState();
    }

    [TearDown]
    public void TearDown()
    {
        Scope.Dispose();
    }

    public TRequestHandler GetRequestHandler<TRequestHandler>()
        where TRequestHandler : IRequestHandler
    {
        return ServiceCollection.GetRequiredService<TRequestHandler>();
    }
}
