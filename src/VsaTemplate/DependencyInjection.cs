using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using VsaTemplate.Common.Models;
using VsaTemplate.Common.Pipeline;
using VsaTemplate.Database;
using VsaTemplate.Features.Users;
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
                (sp, options) =>
                {
                    options.UseSqlite(connectionString);
                    options.ConfigureWarnings(warnings =>
                        warnings.Ignore((RelationalEventId.PendingModelChangesWarning))
                    );
                }
            );

            builder.Services.AddScoped<DatabaseInitialiser>();

            builder
                .Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddIdentityCookies();

            builder.Services.AddAuthorizationBuilder();

            builder
                .Services.AddIdentityCore<User>()
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders()
                .AddApiEndpoints();

            // Web

            builder.Services.AddCors();

            builder.Services.AddExceptionHandler<ExceptionHandler>();

            // Customise default API behaviour
            builder.Services.Configure<ApiBehaviorOptions>(options =>
                options.SuppressModelStateInvalidFilter = true
            );

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddOpenApi();

            /*
            builder.Services.AddOpenApi(options =>
            {
                options.AddOperationTransformer<ApiExceptionOperationTransformer>();
                options.AddOperationTransformer<IdentityApiOperationTransformer>();
            });
            */

            // Services
            builder.Services.AddSingleton(TimeProvider.System);

            builder.Services.AddScoped<CurrentUser>();
        }
    }
}
