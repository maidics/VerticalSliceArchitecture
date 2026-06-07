using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using VsaTemplate.Common.Models;
using VsaTemplate.Infrastructure;

namespace VsaTemplate.Tests.Infrastructure;

public sealed class WebApiFactory(string connectionString) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.UseSetting("ConnectionStrings:VsaTemplateDb", connectionString);

        builder.ConfigureServices(services =>
        {
            services
                .RemoveAll<CurrentUser>()
                .AddTransient(_ =>
                {
                    var mock = new Mock<CurrentUser>();

                    mock.SetupGet(x => x.Roles).Returns(TestApp.GetUserRoles());
                    mock.SetupGet(x => x.Id).Returns(TestApp.GetUserId());

                    return mock.Object;
                });
        });
    }
}
