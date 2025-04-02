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
    
    
    public static DbContext SeedSongs(this DbContext dbx)
    {
        dbx.Set<SongEntity>().AddRange(HotelCF, BillieJean, ShapeOfYou, SongEntityDelete, SongEntityUpdate);
        return dbx;
    }
    
}