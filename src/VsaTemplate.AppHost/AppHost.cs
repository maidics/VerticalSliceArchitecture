using Aspire.Hosting;
using VsaTemplate.AppHost;
using VsaTemplate.Shared;

var builder = DistributedApplication.CreateBuilder(args);

var database = builder.AddSqlite(Services.Database);

var api = builder
    .AddProject<Projects.VsaTemplate>(Services.WebApi)
    .WithReference(database)
    .WaitFor(database)
    .WithExternalHttpEndpoints()
    .WithAspNetCoreEnvironment()
    .WithUrlForEndpoint(
        "http",
        url =>
        {
            url.DisplayText = "Scalar API";
            url.Url = "/scalar";
        }
    );

if (builder.ExecutionContext.IsRunMode)
{
    builder
        .AddJavaScriptApp(Services.WebFrontend, Path.Combine("..", "VsaTemplate", "ClientApp"))
        .WithRunScript("start")
        .WithReference(api)
        .WaitFor(api)
        .WithHttpEndpoint(port: 3000, targetPort: 3000, env: "PORT")
        .WithEnvironment("VITE_API_URL", api.GetEndpoint("http"))
        .WithExternalHttpEndpoints();
}

builder.Build().Run();
