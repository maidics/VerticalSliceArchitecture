using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using VsaTemplate.Common.Extensions;
using VsaTemplate.Common.Models;
using VsaTemplate.Common.Pipeline;
using VsaTemplate.Database;
using VsaTemplate.Features.Users;
using VsaTemplate.Infrastructure;
using VsaTemplate.Shared;

namespace VsaTemplate;

public static class DependencyInjection
{
    extension(IHostApplicationBuilder builder)
    {
        public void AddApplicationServices()
        {
            // Database
            var connectionString = builder.Configuration.GetConnectionString(Services.Database);
            ArgumentException.ThrowIfNullOrEmpty(connectionString);

            builder.Services.AddDbContext<ApplicationDbContext>(
                (_, options) =>
                {
                    options.UseSqlite(connectionString);
                    options.ConfigureWarnings(warnings =>
                        warnings.Ignore((RelationalEventId.PendingModelChangesWarning))
                    );
                }
            );

            builder.Services.AddScoped<DatabaseInitialiser>();

            builder.Services.AddAuthorizationBuilder();

            builder
                .Services.AddIdentityApiEndpoints<ApplicationUser>()
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Web
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    var allowedOrigins =
                        builder.Configuration["CorsOrigins"] ?? "http://localhost:3000";

                    policy
                        .WithOrigins(allowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });
            builder.Services.AddExceptionHandler<ExceptionHandler>();

            // Customise default API behaviour
            builder.Services.Configure<ApiBehaviorOptions>(options =>
                options.SuppressModelStateInvalidFilter = true
            );

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddOpenApi();

            //Features
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Services.AddRequestHandlers();
            builder.Services.AddDomainEventHandlers();

            // Services
            builder.Services.AddSingleton(TimeProvider.System);

            builder.Services.AddScoped<CurrentUser>();
        }
    }
}
