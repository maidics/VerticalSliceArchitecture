using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VsaTemplate.Common.Extensions;
using VsaTemplate.Common.Interfaces;
using VsaTemplate.Common.Pipeline;
using VsaTemplate.Database;
using VsaTemplate.Infrastructure;

namespace VsaTemplate;

public static class DependencyInjection
{
    extension(IHostApplicationBuilder builder)
    {
        public void AddApplicationServices()
        {
            // Database
            builder.AddDatabaseServices();

            // Web
            builder.Services.AddExceptionHandler<ExceptionHandler>();

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
            builder.Services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();
            builder.Services.AddScoped<IUser, CurrentUser>();
        }
    }
}
