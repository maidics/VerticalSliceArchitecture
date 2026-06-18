using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using VsaTemplate.Database.Interceptors;
using VsaTemplate.Features.Users;
using VsaTemplate.Shared;

namespace VsaTemplate.Database;

public static class DependencyInjection
{
    extension(IHostApplicationBuilder builder)
    {
        public void AddDatabaseServices()
        {
            var connectionString = builder.Configuration.GetConnectionString(Services.Database);
            ArgumentException.ThrowIfNullOrEmpty(connectionString);

            builder.Services.AddDbContext<ApplicationDbContext>(
                (sp, options) =>
                {
                    options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
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

            builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();
        }
    }
}
