using ICS_Project.DAL.Entities;
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

    [Fact]
    public void SongInMorePlaylistTest()
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

        var playlist1 = new PlaylistEntity { Name = "Playlist1" };
        var playlist2 = new PlaylistEntity { Name = "Playlist2" };

        // Act
        IcsDbContextSut.Songs.Add(song);
        IcsDbContextSut.Playlists.Add(playlist1);
        IcsDbContextSut.Playlists.Add(playlist2);
        IcsDbContextSut.SaveChanges();
        
        var playlist1Db = IcsDbContextSut.Playlists.FirstOrDefault(p => p.Id == playlist1.Id);
        var playlist2Db = IcsDbContextSut.Playlists.FirstOrDefault(p => p.Id == playlist2.Id);
        
        playlist1Db?.AddSongItem(song);
        playlist2Db?.AddSongItem(song);
        
        IcsDbContextSut.SaveChanges();
        
        var songInPlaylist1 = IcsDbContextSut.Songs.FirstOrDefault(s => s.Id == song.Id);
        var songInPlaylist2 = IcsDbContextSut.Songs.FirstOrDefault(s => s.Id == song.Id);

        // Assert
        Assert.NotNull(songInPlaylist1);
        Assert.NotNull(songInPlaylist2);
    }
}