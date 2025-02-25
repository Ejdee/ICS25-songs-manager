using ICS_Project.DAL.Factories;

namespace ICS_Project.DAL.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var dbContextFactory= new DbContextSqLiteFactory("test");
        var dbx = dbContextFactory.CreateDbContext();
        dbx.Database.EnsureCreated();



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
    }
}