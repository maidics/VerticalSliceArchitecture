using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using VsaTemplate.Features.Examples;
using VsaTemplate.Features.Users;

namespace VsaTemplate.Infrastructure.Database;

public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Example> Examples => Set<Example>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
