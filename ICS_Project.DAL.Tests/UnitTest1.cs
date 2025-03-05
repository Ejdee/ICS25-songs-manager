using ICS_Project.DAL.Factories;

namespace ICS_Project.DAL.Tests;

public class UnitTest1
{
    public UnitTest1()
    {
        var dbContextFactory= new DbContextSqLiteFactory("test");
        IcsDbContextSut = dbContextFactory.CreateDbContext();
        IcsDbContextSut.Database.EnsureDeleted();
        IcsDbContextSut.Database.EnsureCreated();
    }

    public IcsDbContext IcsDbContextSut { get; set; }

    [Fact]
    public void Test1()
    {
        var author = new ArtistEntity(
            name: "John Doe",
            description: "John Doe",
            country: "USA"
        );
        var album = new AlbumEntity(
            name: "Test AlbumEntity",
            description: "Test AlbumEntity",
            releaseYear: 2004,
            artistEntity: author
        );
        var song = new SongEntity(
            name: "Test SongEntity",
            description: "Test SongEntity Description",
            genre: "Test SongEntity Genre",
            durationInSeconds: 10,
            artistEntity: author,
            albumEntity: album,
            songUrl: "testurl"
        );
        
        IcsDbContextSut.Songs.Add(song);
        IcsDbContextSut.Albums.Add(album);
        IcsDbContextSut.Artists.Add(author);
        IcsDbContextSut.SaveChanges();


        var SongFromDb = IcsDbContextSut.Songs.FirstOrDefault(song1 => song1.Id == song.Id);
        Assert.Equal(song, SongFromDb);
    }
}