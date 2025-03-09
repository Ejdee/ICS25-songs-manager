using Xunit;

namespace ICS_Project.DAL.Tests;

[Collection("Sequential")]
public class PlaylistTests : IntegrationTestBase
{
    [Fact]
    public void AddPlaylistTest()
    {
        // Arrange
        var playlist = new PlaylistEntity { Name = "Test Playlist" };
        
        // Act
        IcsDbContextSut.Playlists.Add(playlist);
        IcsDbContextSut.SaveChanges();
        
        // Assert
        var playlistFromDb = IcsDbContextSut.Playlists.FirstOrDefault(playlist1 => playlist1.Id == playlist.Id);    
        Assert.Equal(playlist, playlistFromDb);
    }

    [Fact]
    public void AddSongToPlaylistTest()
    {
        // Arrange
        var playlist = new PlaylistEntity { Name = "Test Playlist" };
        
        var song = new SongEntity(
            name: "Test SongEntity",
            description: "Test SongEntity Description",
            genre: "Test SongEntity Genre",
            durationInSeconds: 10,
            artist: "John Doe",
            songUrl: "testurl"
        );
        
        // Act
        IcsDbContextSut.Playlists.Add(playlist);
        IcsDbContextSut.Songs.Add(song);
        IcsDbContextSut.SaveChanges();
        
        playlist.AddSongItem(song);
        IcsDbContextSut.SaveChanges();
        
        // Assert
        var playlistFromDb = IcsDbContextSut.Playlists.FirstOrDefault(playlist1 => playlist1.Id == playlist.Id);    
        Assert.Equal(playlist, playlistFromDb);
        Assert.Single(playlist.Songs);
        Assert.Equal(playlist.Songs.Count, playlistFromDb?.Songs.Count);
        Assert.Equal(playlist.Songs, playlistFromDb?.Songs);
        Assert.Equal(song.Name, playlistFromDb?.Songs[0].Name);
    }

    [Fact]
    public void GetSongsFromPlaylistTest()
    {
        // Arrange
        var playlist = new PlaylistEntity { Name = "Test Playlist" };
        var song1 = new SongEntity(
            name: "First SongEntity",
            description: "Test SongEntity Description",
            genre: "Test SongEntity Genre",
            durationInSeconds: 10,
            artist: "John Doe",
            songUrl: "testurl"
        );
        var song2 = new SongEntity(
            name: "Second SongEntity",
            description: "Test SongEntity Description",
            genre: "Test SongEntity Genre",
            durationInSeconds: 10,
            artist: "John Doe",
            songUrl: "testurl"
        );
        
        // Act
        IcsDbContextSut.Playlists.Add(playlist);
        IcsDbContextSut.Songs.Add(song1);
        IcsDbContextSut.Songs.Add(song2);
        IcsDbContextSut.SaveChanges();
        
        var playlistDb = IcsDbContextSut.Playlists.FirstOrDefault(p => p.Id == playlist.Id);
        var s1 = IcsDbContextSut.Songs.FirstOrDefault(s => s.Id == song1.Id);
        var s2 = IcsDbContextSut.Songs.FirstOrDefault(s => s.Id == song2.Id);
        
        // Assert  
        Assert.NotNull(s1); 
        Assert.NotNull(s2);
        Assert.NotNull(playlistDb);
        
        playlistDb.AddSongItem(s1);
        playlistDb.AddSongItem(s2);
        IcsDbContextSut.SaveChanges();

        Assert.Equal(2, playlistDb.Songs.Count);
        Assert.Collection(playlistDb.Songs,
            song => Assert.Equal(song1.Id, song.Id),
            song => Assert.Equal(song2.Id, song.Id)
        );
    }
}