namespace VsaTemplate.Tests.Infrastructure;

public abstract class TestBase
{
    [SetUp]
    public async Task Setup()
    {
        await TestApp.ResetState();
    }
}
