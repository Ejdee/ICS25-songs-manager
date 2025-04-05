using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ICS_Project.DAL.Entities;
using ICS_Project.Common.Tests;
using ICS_Project.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Xunit;
using Xunit.Abstractions;

namespace ICS_Project.DAL.Tests;

[Collection("Sequential")]
public class PlaylistTests(ITestOutputHelper output) : IntegrationTestBase(output)
{
    [Fact]
    public async Task AddPlaylistTest()
    {
        // Arrange
        var playlist = PlaylistSeeds.EmptyPlaylist with
        {
            Name = "Test Playlist"
        };

        // Act
        IcsDbContextSut.Playlists.Add(playlist);
        await IcsDbContextSut.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualPlaylist = await dbx.Playlists.SingleAsync(p => p.Id == playlist.Id);
        DeepAssert.Equal(playlist, actualPlaylist);
    }

    [Fact]
    public async Task AddSongToPlaylistTest()
    {
        // Arrange
        var playlist = PlaylistSeeds.EmptyPlaylist with
        {
            Name = "Workout Mix",
            PlaylistSongs = new List<PlaylistSongEntity>
            {
                PlaylistSongSeeds.EmptyPlaylistSong with
                {
                    Song = SongSeeds.EmptySongEntity with
                    {
                        Name = "Eye of the Tiger",
                        Description = "Eye of the Tiger by Survivor",
                        Artist = "Survivor",
                        DurationInSeconds = 245,
                        Genre = "Rock",
                        SongUrl = "https://example.com/eye_of_the_tiger.mp3"
                    }
                },
                PlaylistSongSeeds.EmptyPlaylistSong with
                {
                    Song = SongSeeds.EmptySongEntity with
                    {
                        Name = "Lose Yourself",
                        Description = "Song by Eminem",
                        Artist = "Eminem",
                        DurationInSeconds = 326,
                        Genre = "Hip-Hop",
                        SongUrl = "https://example.com/lose_yourself.mp3"
                    }
                }
            }
        };

        // Act
        IcsDbContextSut.Playlists.Add(playlist);
        await IcsDbContextSut.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var playlistFromDb = await dbx.Playlists
            .Include(p => p.PlaylistSongs)
            .ThenInclude(p => p.Song)
            .SingleAsync(p => p.Id == playlist.Id);

        DeepAssert.Equal(playlistFromDb, playlist);
    }

    [Fact]
    public async Task AddNew_PlaylistWithJustSongs()
    {
        // Arrange
        var playlist = PlaylistSeeds.EmptyPlaylist with
        {
            Name = "Test Playlist",
            PlaylistSongs = new List<PlaylistSongEntity>
            {
                PlaylistSongSeeds.EmptyPlaylistSong with
                {
                    SongId = SongSeeds.ShapeOfYou.Id
                },
                PlaylistSongSeeds.EmptyPlaylistSong with
                {
                    SongId = SongSeeds.BillieJean.Id
                }
            }
        };
        
        // Act
        IcsDbContextSut.Playlists.Add(playlist);
        await IcsDbContextSut.SaveChangesAsync();
        
        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualPlaylist = await dbx.Playlists
            .Include(p => p.PlaylistSongs)
            .SingleAsync(p => p.Id == playlist.Id);
        
        DeepAssert.Equal(playlist, actualPlaylist);
    }

    [Fact]
    public async Task GetAll_Playlists_ContainingSong()
    {
        // Act
        var playlists = await IcsDbContextSut.Playlists.ToListAsync();
        
        // Assert
        DeepAssert.Contains(PlaylistSeeds.PopPlaylist, playlists, nameof(PlaylistEntity.PlaylistSongs));
    }
    
    [Fact]
    public async Task GetById_Playlist()
    {
        // Act
        var playlist = await IcsDbContextSut.Playlists.SingleAsync(p => p.Id == PlaylistSeeds.PopPlaylist.Id);
        
        // Assert
        DeepAssert.Equal(PlaylistSeeds.PopPlaylist with { PlaylistSongs = Array.Empty<PlaylistSongEntity>() }, playlist);
    }

    [Fact]
    public async Task Update_Playlist_Persisted()
    {
        // Arrange
        var basePlaylist = PlaylistSeeds.PopPlaylistUpdate;
        var playlist =
            basePlaylist with
            {
                Name = basePlaylist.Name + " Updated"
            };

        // Act
        IcsDbContextSut.Playlists.Update(playlist);
        await IcsDbContextSut.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Playlists.SingleAsync(i => i.Id == playlist.Id);
        DeepAssert.Equal(playlist, actualEntity);
    }
    
    [Fact]
    public async Task Delete_PlaylistWithoutSongs_Deleted()
    {
        // Arrange
        var basePlaylist = PlaylistSeeds.PopPlaylistDelete;

        // Act
        IcsDbContextSut.Playlists.Remove(basePlaylist);
        await IcsDbContextSut.SaveChangesAsync();

        // Assert
        Assert.False(await IcsDbContextSut.Playlists.AnyAsync(i => i.Id == basePlaylist.Id));
    }
    
    [Fact]
    public async Task DeleteById_PlaylistWithoutSongs_Deleted()
    {
        // Arrange
        var basePlaylist = PlaylistSeeds.PopPlaylistDelete;

        // Act
        IcsDbContextSut.Remove(
            IcsDbContextSut.Playlists.Single(i => i.Id == basePlaylist.Id));
        await IcsDbContextSut.SaveChangesAsync();

        // Assert
        Assert.False(await IcsDbContextSut.Playlists.AnyAsync(i => i.Id == basePlaylist.Id));
    } 
    
    [Fact]
    public async Task Delete_PlaylistWithPlaylistSongs_Deleted()
    {
        // Arrange
        var basePlaylist = PlaylistSeeds.PopPlaylistUpdate;

        // Act
        IcsDbContextSut.Playlists.Remove(basePlaylist);
        await IcsDbContextSut.SaveChangesAsync();

        // Assert
        Assert.False(await IcsDbContextSut.Playlists.AnyAsync(i => i.Id == basePlaylist.Id));
        Assert.False(await IcsDbContextSut.PlaylistSongs
            .AnyAsync(i => basePlaylist.PlaylistSongs.Select(playlistSong => playlistSong.Id).Contains(i.Id)));
    } 
}