using ICS_Project.BL.Facades;
using ICS_Project.BL.Facades.Interfaces;
using ICS_Project.BL.Models;
using ICS_Project.Common.Tests.Seeds;
using Xunit.Abstractions;

namespace ICS_Project.BL.Tests;

public class PlaylistSongFacadeTests : FacadeTestBase
{
    private readonly IPlaylistSongFacade _playlistSongFacadeSUT; 
    private readonly IPlaylistFacade _playlistFacadeSUT;
    
    public PlaylistSongFacadeTests(ITestOutputHelper output) : base(output)
    {
        _playlistSongFacadeSUT = new PlaylistSongFacade(UnitOfWorkFactory, PlaylistSongModelMapper);
        _playlistFacadeSUT = new PlaylistFacade(UnitOfWorkFactory, PlaylistModelMapper);
    }
    
    [Fact]
    public async Task AddSongToPlaylist_AddNew()
    {
        //Arrange
        var newPlaylistSong = new PlaylistSongDetailModel
        {
            Id = Guid.NewGuid(),
            SongId = SongSeeds.BillieJean.Id,
            SongName = SongSeeds.BillieJean.Name,
            SongUrl = SongSeeds.BillieJean.SongUrl!,
            SongDurationInSeconds = TimeSpan.FromSeconds(SongSeeds.BillieJean.DurationInSeconds),
            Artist = SongSeeds.BillieJean.Artist,
            Genre = SongSeeds.BillieJean.Genre
        };

        //Act
        await _playlistSongFacadeSUT.SaveAsync(newPlaylistSong, PlaylistSeeds.PlaylistA.Id);
        var playlist = await _playlistFacadeSUT.GetAsync(PlaylistSeeds.PlaylistA.Id);

        //Assert
        Assert.NotNull(playlist);
        Assert.NotNull(playlist.Songs);
        Assert.Contains(playlist.Songs, s => s.SongId == newPlaylistSong.SongId);
    }

    [Fact]
    public async Task DeleteSongFromPlaylist_Existing()
    {
        //Arrange
        var playlist = await _playlistFacadeSUT.GetAsync(PlaylistSeeds.PopPlaylist.Id);
        var playlistSongCount = playlist?.Songs.Count ?? 0;
        var songToDelete = playlist?.Songs.FirstOrDefault(s => s.SongId == SongSeeds.BillieJean.Id);

        //Act
        if (songToDelete != null)
        {
            await _playlistSongFacadeSUT.DeleteAsync(songToDelete.Id);
        }
        
        var updatedPlaylist = await _playlistFacadeSUT.GetAsync(PlaylistSeeds.PopPlaylist.Id);
        var updatedPlaylistSongCount = updatedPlaylist?.Songs.Count ?? 0;

        //Assert
        Assert.NotNull(updatedPlaylist);
        Assert.True(updatedPlaylistSongCount < playlistSongCount);
        Assert.DoesNotContain(updatedPlaylist.Songs, s => s.SongId == SongSeeds.BillieJean.Id);
    }
    
    [Fact]
    public async Task DeleteSongFromPlaylist_NonExisting_Throws()
    {
        //Arrange
        var nonExistingId = Guid.NewGuid();

        //Act & Assert
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _playlistSongFacadeSUT.DeleteAsync(nonExistingId));
    }


    [Fact]
    public async Task addNewSongPlaylist_ListModel_Existing_Update()
    {
        //Arrange
        var uid = Guid.NewGuid();
        var existingPlaylistSong = new PlaylistSongDetailModel()
        {
            Id = uid,
            SongId = SongSeeds.BillieJean.Id,
            SongName = SongSeeds.BillieJean.Name,
            SongDurationInSeconds = TimeSpan.FromSeconds(SongSeeds.BillieJean.DurationInSeconds),
            Artist = SongSeeds.BillieJean.Artist,
            Genre = SongSeeds.BillieJean.Genre,
            SongUrl = SongSeeds.BillieJean.SongUrl!
        };
        
        //Act
        await _playlistSongFacadeSUT.SaveAsync(existingPlaylistSong, PlaylistSeeds.PlaylistZ.Id);

        var updated = new PlaylistSongListModel()
        {
            Id = uid,
            SongId = SongSeeds.ShapeOfYou.Id,
            SongName = SongSeeds.ShapeOfYou.Name,
            SongDurationInSeconds = TimeSpan.FromSeconds(SongSeeds.ShapeOfYou.DurationInSeconds)
        };
        
        await _playlistSongFacadeSUT.SaveAsync(updated, PlaylistSeeds.PlaylistZ.Id);
        
        var playlist = await _playlistFacadeSUT.GetAsync(PlaylistSeeds.PlaylistZ.Id);
        
        //Assert
        Assert.NotNull(playlist);
        Assert.NotNull(playlist.Songs);
        Assert.Contains(playlist.Songs, s => s.SongId == updated.SongId);
    }
    
    [Fact]
    public async Task MapSongToExistingDetail_ExistingSong()
    {
        //Arrange
        var newSong = new SongListModel()
        {
            Name = SongSeeds.SongA.Name,
            Id = SongSeeds.SongA.Id,
            DurationInSeconds = TimeSpan.FromSeconds(SongSeeds.SongA.DurationInSeconds)
        };
        
        var detailModel = PlaylistSongModelMapper.MapToDetailModel(PlaylistSongSeeds.EmptyPlaylistSong);
        
        //Act 
        PlaylistSongModelMapper.MapToExistingDetailModel(detailModel, newSong);
        await _playlistSongFacadeSUT.SaveAsync(detailModel, PlaylistSeeds.PlaylistA.Id);
        var playlist = await _playlistFacadeSUT.GetAsync(PlaylistSeeds.PlaylistA.Id);
        
        //Assert
        Assert.NotNull(playlist);
        Assert.Contains(playlist.Songs, s => s.SongId == newSong.Id);
    }
}