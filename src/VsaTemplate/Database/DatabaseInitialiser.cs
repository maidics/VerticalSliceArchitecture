namespace VsaTemplate.Database;

public sealed class DatabaseInitialiser(ApplicationDbContext context)
{
    public async Task InitialiseAsync()
    {
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }
}
