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
        var author = new Artist(
            name: "John Doe",
            description: "John Doe",
            country: "USA"
        );
        var album = new Album(
            name: "Test Album",
            description: "Test Album",
            releaseYear: 2004,
            artist: author
        );
        var song = new Song(
            name: "Test Song",
            description: "Test Song Description",
            genre: "Test Song Genre",
            durationInSeconds: 10,
            artist: author,
            album: album,
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