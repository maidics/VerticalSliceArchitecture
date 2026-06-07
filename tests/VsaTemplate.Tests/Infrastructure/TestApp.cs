using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VsaTemplate.Common.Interfaces;
using VsaTemplate.Database;
using VsaTemplate.Features.Users;

namespace VsaTemplate.Tests.Infrastructure;

public static class TestApp
{
    private static HttpClient? _httpClient;

    private static Guid? _userId;
    private static ImmutableArray<string> _roles;

    public static Guid? GetUserId() => _userId;

    public static ImmutableArray<string> GetUserRoles() => _roles;

    public static async Task ResetState()
    {
        if (SetUp.Database is not null)
            await SetUp.Database.ResetAsync();

        _httpClient?.Dispose();

        _userId = null;
        _roles = [];

        using var scope = SetUp.ScopeFactory.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<
            RoleManager<IdentityRole<Guid>>
        >();

        foreach (var role in Roles.All)
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }

        _httpClient = SetUp.CreateHttpClient();
    }

    public static async Task<Guid> RunAsUserAsync(string email, string password, string[] roles)
    {
        if (roles.Any(role => !Roles.IsValid(role)))
            throw new ArgumentException(
                $"Invalid role: {string.Join(", ", roles.Where(role => !Roles.IsValid(role)))}"
            );

        using var scope = SetUp.ScopeFactory.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var user = new ApplicationUser { UserName = email, Email = email };

        var creationResult = await userManager.CreateAsync(user, password);

        if (!creationResult.Succeeded)
            throw new InvalidOperationException($"Failed to create user: {email}");

        if (roles.Length > 1)
        {
            var roleResult = await userManager.AddToRolesAsync(user, roles);

            if (!roleResult.Succeeded)
                throw new InvalidOperationException(
                    $"Failed to add user to roles: {email}. Roles: {string.Join(", ", roles)}."
                );
        }

        return user.Id;
    }

    public static async Task<HttpResponseMessage> SendPostAsync(string uri, IRequest request)
    {
        ArgumentNullException.ThrowIfNull(_httpClient);

        return await _httpClient.PostAsJsonAsync(uri, request);
    }

    public static async Task<HttpResponseMessage> SendGetAsync(
        [StringSyntax(StringSyntaxAttribute.Uri)] string uri
    )
    {
        ArgumentNullException.ThrowIfNull(_httpClient);

        return await _httpClient.GetAsync(uri);
    }

    public static async Task<HttpResponseMessage> SendPutAsync(string uri, IRequest request)
    {
        ArgumentNullException.ThrowIfNull(_httpClient);

        return await _httpClient.PutAsJsonAsync(uri, request);
    }

    public static async Task<HttpResponseMessage> SendDeleteAsync(string uri)
    {
        ArgumentNullException.ThrowIfNull(_httpClient);

        return await _httpClient.DeleteAsync(uri);
    }

    public static async Task<TEntity?> FirstOrDefaultAsync<TEntity>(
        Expression<Func<TEntity, bool>> expression
    )
        where TEntity : class
    {
        using var scope = SetUp.ScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Set<TEntity>().FirstOrDefaultAsync(expression);
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = SetUp.ScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.AddAsync(entity);

        await context.SaveChangesAsync();
    }

    public static async Task<int> CountAsync<TEntity>()
        where TEntity : class
    {
        using var scope = SetUp.ScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Set<TEntity>().CountAsync();
    }
}
