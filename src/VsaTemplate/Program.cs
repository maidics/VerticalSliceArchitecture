using Scalar.AspNetCore;
using VsaTemplate;
using VsaTemplate.Common.Extensions;
using VsaTemplate.Common.Pipeline;
using VsaTemplate.Database;
using VsaTemplate.Features.Users;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    using var scope = app.Services.CreateScope();

    var initialiser = scope.ServiceProvider.GetRequiredService<DatabaseInitialiser>();
    await initialiser.InitialiseAsync();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors(static builder => builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());

app.UseFileServer();

app.UseExceptionHandler(options => { });

app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("/api")
    .AddEndpointFilter<LoggingFilter>()
    .AddEndpointFilter<ValidationFilter>()
    .AddEndpointFilter<PerformanceFilter>()
    .MapEndpoints();

//app.MapDefaultEndpoints(); // ServiceDefaults observability
app.MapGroup("/api/identity") /* TODO: add logging without logging credentials */
    .MapIdentityApi<ApplicationUser>()
    .WithTags("Users");

app.MapOpenApi();
app.MapScalarApiReference();
app.MapFallbackToFile("index.html");

app.Run();

namespace VsaTemplate
{
    public partial class Program;
}
