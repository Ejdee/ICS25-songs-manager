using System.Linq;
using System.Threading.Tasks;
using ICS_Project.Common.Tests;
using ICS_Project.Common.Tests.Seeds;
using ICS_Project.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace ICS_Project.DAL.Tests;

public class DbContextPlaylistSongTests(ITestOutputHelper output) : IntegrationTestBase(output)
{
    [Fact]
    public async Task GetAll_PlaylistSongs_ForPlaylist()
    {
        // Act
        var playlistSongs = await IcsDbContextSut.PlaylistSongs
            .Where(ps => ps.PlaylistId == PlaylistSeeds.PopPlaylist.Id)
            .ToArrayAsync();

        // Assert
        DeepAssert.Contains(PlaylistSongSeeds.PopPlaylistSongs with { Playlist = null!, Song = null! }, playlistSongs);
        DeepAssert.Contains(PlaylistSongSeeds.PopPlaylistSongs2 with { Playlist = null!, Song = null! }, playlistSongs);
    }

    [Fact]
    public async Task GetAll_PlaylistSongs_IncludingSongs_ForPlaylist()
    {
        // Act
        var playlistSongs = await IcsDbContextSut.PlaylistSongs
            .Where(ps => ps.PlaylistId == PlaylistSongSeeds.PopPlaylistSongs.PlaylistId)
            .Include(ps => ps.Song)
            .ToArrayAsync();

        // Assert
        DeepAssert.Contains(PlaylistSongSeeds.PopPlaylistSongs with { Playlist = null! }, playlistSongs);
        DeepAssert.Contains(PlaylistSongSeeds.PopPlaylistSongs2 with { Playlist = null! }, playlistSongs);
    }

    [Fact]
    public async Task Update_PlaylistSong_Persisted()
    {
        // Arrange
        var baseEntity = PlaylistSongSeeds.PopPlaylistSongsUpdate;
        var entity = baseEntity with { SongId = SongSeeds.ShapeOfYou.Id, Playlist = null!, Song = null! };

        // Act
        IcsDbContextSut.PlaylistSongs.Update(entity);
        await IcsDbContextSut.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.PlaylistSongs.SingleAsync(ps => ps.Id == entity.Id);
        Assert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task Delete_PlaylistSong_Deleted()
    {
        // Arrange
        var baseEntity = PlaylistSongSeeds.PopPlaylistSongsDelete;

        // Act
        IcsDbContextSut.PlaylistSongs.Remove(baseEntity);
        await IcsDbContextSut.SaveChangesAsync();

        // Assert
        Assert.False(await IcsDbContextSut.PlaylistSongs.AnyAsync(ps => ps.Id == baseEntity.Id));
    }

    [Fact]
    public async Task DeleteById_PlaylistSong_Deleted()
    {
        // Arrange
        var baseEntity = PlaylistSongSeeds.PopPlaylistSongsDelete;

        // Act
        IcsDbContextSut.Remove(
            IcsDbContextSut.PlaylistSongs.Single(ps => ps.Id == baseEntity.Id));
        await IcsDbContextSut.SaveChangesAsync();

        // Assert
        Assert.False(await IcsDbContextSut.PlaylistSongs.AnyAsync(ps => ps.Id == baseEntity.Id));
    }
}