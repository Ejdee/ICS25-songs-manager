using ICS_Project.DAL.Factories;

namespace ICS_Project.DAL.Tests;

public class IntegrationTestBase : IAsyncLifetime
{
    protected IntegrationTestBase()
    {
        var dbContextFactory= new DbContextSqLiteFactory("test");
        IcsDbContextSut = dbContextFactory.CreateDbContext();
    }

    protected IcsDbContext IcsDbContextSut { get; set; } 
    public async Task InitializeAsync()
    {
        await IcsDbContextSut.Database.EnsureDeletedAsync();
        await IcsDbContextSut.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await IcsDbContextSut.DisposeAsync();
    }
}