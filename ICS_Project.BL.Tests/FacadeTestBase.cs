using ICS_Project.BL.Mappers;
using ICS_Project.Common.Tests;
using ICS_Project.Common.Tests.Seeds;
using ICS_Project.DAL;
using ICS_Project.DAL.Factories;
using ICS_Project.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace ICS_Project.BL.Tests;

public class FacadeTestBase : IAsyncLifetime
{
    protected FacadeTestBase(ITestOutputHelper output)
    {
        XUnitTestOutputConverter converter = new(output);
        Console.SetOut(converter);
        
        DbContextFactory = new DbContextSqLiteFactory(GetType().FullName!);

        SongModelMapper = new SongModelMapper();
        PlaylistSongModelMapper = new PlaylistSongModelMapper();
        PlaylistModelMapper = new PlaylistModelMapper(PlaylistSongModelMapper);

        UnitOfWorkFactory = new UnitOfWorkFactory(DbContextFactory);
    } 
    
    protected IDbContextFactory<IcsDbContext> DbContextFactory { get; }
    protected SongModelMapper SongModelMapper { get; }
    protected PlaylistSongModelMapper PlaylistSongModelMapper { get; }
    protected PlaylistModelMapper PlaylistModelMapper { get; }
    protected IUnitOfWorkFactory UnitOfWorkFactory { get; }
    
    public async Task InitializeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeletedAsync();
        await dbx.Database.EnsureCreatedAsync();
        
        dbx
            .SeedSongs()
            .SeedPlaylists()
            .SeedPlaylistSongs();
        await dbx.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeletedAsync(); 
    }
}