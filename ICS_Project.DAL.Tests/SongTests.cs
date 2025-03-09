using Xunit;

namespace ICS_Project.DAL.Tests;

[Collection("Sequential")]
public class SongTests : IntegrationTestBase
{
    [Fact]
    public void AddSongTest()
    {
        // Arrange
        var song = new SongEntity(
            name: "Test SongEntity",
            description: "Test SongEntity Description",
            genre: "Test SongEntity Genre",
            durationInSeconds: 10,
            artist: "John Doe",
            songUrl: "testurl"
        );
        
        // Act
        IcsDbContextSut.Songs.Add(song);
        IcsDbContextSut.SaveChanges();

        // Assert
        var songFromDb = IcsDbContextSut.Songs.FirstOrDefault(song1 => song1.Id == song.Id);
        Assert.Equal(song, songFromDb);
    }

    [Fact]
    public void UpdateSongNameTest()
    {
        // Arrange
        var song = new SongEntity(
            name: "Test SongEntity",
            description: "Test SongEntity Description",
            genre: "Test SongEntity Genre",
            durationInSeconds: 10,
            artist: "John Doe",
            songUrl: "testurl"
        );
        IcsDbContextSut.Songs.Add(song);
        IcsDbContextSut.SaveChanges();

        // Act
        var songFromDb = IcsDbContextSut.Songs.FirstOrDefault(s => s.Id == song.Id);
        if (songFromDb != null) songFromDb.Name = "Updated SongEntity Name";
        IcsDbContextSut.SaveChanges();

        // Assert
        var updatedSong = IcsDbContextSut.Songs.FirstOrDefault(s => s.Id == song.Id);
        Assert.Equal("Updated SongEntity Name", updatedSong?.Name);
    }

    [Fact]
    public void DeleteSongTest()
    {
        // Arrange
        var song = new SongEntity(
            name: "Test SongEntity",
            description: "Test SongEntity Description",
            genre: "Test SongEntity Genre",
            durationInSeconds: 10,
            artist: "John Doe",
            songUrl: "testurl"
        );
        IcsDbContextSut.Songs.Add(song);
        IcsDbContextSut.SaveChanges(); 
        
        // Act
        var songFromDb = IcsDbContextSut.Songs.FirstOrDefault(s => s.Id == song.Id);
        Assert.Equal(song, songFromDb);
        
        IcsDbContextSut.Songs.Remove(song);
        IcsDbContextSut.SaveChanges();
        
        // Assert
        var deletedSong = IcsDbContextSut.Songs.FirstOrDefault(s => s.Id == song.Id);
        Assert.Null(deletedSong);
    }
}