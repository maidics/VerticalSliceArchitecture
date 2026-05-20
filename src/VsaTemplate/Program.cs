using Scalar.AspNetCore;
using VsaTemplate;
using VsaTemplate.Common.Extensions;
using VsaTemplate.Database;
using VsaTemplate.Features.Users;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
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

//app.MapDefaultEndpoints(); // ServiceDefaults observability
app.MapGroup("/api/identity").MapIdentityApi<User>().WithTags("Users");
app.MapEndpoints();
app.MapOpenApi();
app.MapScalarApiReference();
app.MapFallbackToFile("index.html"); //TODO: what does this do?

app.Run();
