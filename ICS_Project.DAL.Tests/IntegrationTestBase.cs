using System;
using System.Threading.Tasks;
using ICS_Project.Common.Tests;
using ICS_Project.Common.Tests.Seeds;
using ICS_Project.DAL.Factories;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace ICS_Project.DAL.Tests;

public class IntegrationTestBase : IAsyncLifetime
{
    protected IntegrationTestBase(ITestOutputHelper output)
    {
        XUnitTestOutputConverter converter = new(output);
        Console.SetOut(converter);

        DbContextFactory = new DbContextSqLiteFactory(GetType().FullName!);
        IcsDbContextSut = DbContextFactory.CreateDbContext();
    }

    protected IDbContextFactory<IcsDbContext> DbContextFactory { get; }
    protected IcsDbContext IcsDbContextSut { get; }
    
    public async Task InitializeAsync()
    {
        await IcsDbContextSut.Database.EnsureDeletedAsync();
        await IcsDbContextSut.Database.EnsureCreatedAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        dbx
            .SeedSongs()
            .SeedPlaylists()
            .SeedPlaylistSongs();
        await dbx.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await IcsDbContextSut.Database.EnsureDeletedAsync();
        await IcsDbContextSut.DisposeAsync();
    }

}