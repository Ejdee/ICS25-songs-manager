using ICS_Project.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS_Project.Common.Tests.Seeds;

public static class SongSeeds
{
    public static readonly SongEntity EmptySongEntity = new()
    {
        Id = default,
        Name = default!,
        Description = default!,
        Genre = default!,
        DurationInSeconds = default,
        Artist = default!,
        SongUrl = default!
    };

    public static readonly SongEntity HotelCF = new()
    {
        Id = Guid.Parse("5137d6af-d896-45c1-b478-cb5de5664864"),
        Name = "Hotel California",
        Description = "Welcome to the Hotel California",
        Genre = "Rock",
        DurationInSeconds = 390,
        Artist = "Eagles",
        SongUrl = "someUrl.com"
    };

    public static readonly SongEntity SongEntityUpdate = HotelCF with { Id = Guid.Parse("be1e5b65-ebf3-423c-8d68-c3c7998832b6") };
    public static readonly SongEntity SongEntityDelete = HotelCF with { Id = Guid.Parse("0d0a9a8b-dd04-4e39-9948-524ea2c1b15c") };
    
    public static readonly SongEntity ShapeOfYou = new()
    {
        Id = Guid.Parse("12bc8bfb-1445-48db-a7b7-547871a0d9cf"),
        Name = "Shape of You",
        Description = "The club isn't the best place to find a lover",
        Genre = "Pop",
        DurationInSeconds = 233,
        Artist = "Ed Sheeran",
        SongUrl = "someUrl.com"
    };
    
    public static readonly SongEntity BillieJean = new()
    {
        Id = Guid.Parse("08c8a477-414c-4712-bdae-fd564eb727d5"),
        Name = "Billie Jean",
        Description = "Billie Jean is not my lover",
        Genre = "Pop",
        DurationInSeconds = 294,
        Artist = "Michael Jackson",
        SongUrl = "someUrl.com"
    };
    
    public static readonly SongEntity SongA = new()
    {
        Id = Guid.Parse("ee4b2a65-b052-4bad-b813-8b50c4feff46"),
        Name = "A",
        Description = "Alphabetical sort test",
        Genre = "AlphaGenre",
        DurationInSeconds = 100,
        Artist = "Michael Jackson",
        SongUrl = "someUrl.com"
    };
    
    public static readonly SongEntity SongB = new()
    {
        Id = Guid.Parse("eb37916f-6d66-4e12-9723-7d78ceb23e03"),
        Name = "B",
        Description = "Alphabetical sort test2",
        Genre = "BetaGenre",
        DurationInSeconds = 200,
        Artist = "Michael Jackson",
        SongUrl = "someUrl.com"
    };
    
    public static readonly SongEntity SongC = new()
    {
        Id = Guid.Parse("d6c7ab96-8a41-44f7-b6b6-e8eae8e04b41"),
        Name = "C",
        Description = "Alphabetical sort test3",
        Genre = "AlphaGenre",
        DurationInSeconds = 300,
        Artist = "Michael Jackson",
        SongUrl = "someUrl.com"
    };
    
    public static readonly SongEntity SongZ = new()
    {
        Id = Guid.Parse("aac70000-8a41-44f7-b6b6-e8eae8e04b41"),
        Name = "Z",
        Description = "Alphabetical sort test3",
        Genre = "AlphaGenre",
        DurationInSeconds = 400,
        Artist = "Michael Jackson",
        SongUrl = "someUrl.com"
    };
    
    public static DbContext SeedSongs(this DbContext dbx)
    {
        dbx.Set<SongEntity>().AddRange(
            HotelCF,
            BillieJean,
            ShapeOfYou,
            SongEntityDelete,
            SongEntityUpdate,
            SongA,
            SongB,
            SongC,
            SongZ
            );
        return dbx;
    }
    
}