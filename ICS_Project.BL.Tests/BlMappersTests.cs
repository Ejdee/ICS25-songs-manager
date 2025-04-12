using ICS_Project.Common.Tests.Seeds;
using ICS_Project.DAL.Entities;
using Xunit.Abstractions;

namespace ICS_Project.BL.Tests;

public class BlMappersTests : FacadeTestBase
{
    public BlMappersTests(ITestOutputHelper output) : base(output)
    {
        
    }

    [Fact]
    public void MapToListModel_Songs_IEnumerable()
    {
        //Arrange
        var songList = new List<SongEntity>
        {
            SongSeeds.BillieJean,
            SongSeeds.ShapeOfYou,
            SongSeeds.SongZ
        };
        
        //Act
        var mappedList = SongModelMapper.MapToListModel(songList);
        
        //Assert
        Assert.NotNull(mappedList);
        Assert.Equal(songList.Count, mappedList.Count());
    } 

    [Fact]
    public void MapToDetailModel_PlaylistSong_GetValues()
    {
        //Arrange
        var detailModel = PlaylistSongModelMapper.MapToDetailModel(PlaylistSongSeeds.PlaylistSongA);
        
        //Assert
        Assert.Equal(PlaylistSongSeeds.PlaylistSongA.Id, detailModel.Id);
        Assert.Equal(PlaylistSongSeeds.PlaylistSongA.SongId, detailModel.SongId);
        Assert.Equal(PlaylistSongSeeds.PlaylistSongA.Song.Name, detailModel.SongName);
        Assert.Equal(PlaylistSongSeeds.PlaylistSongA.Song.Artist, detailModel.Artist);
        Assert.Equal(PlaylistSongSeeds.PlaylistSongA.Song.Genre, detailModel.Genre);
        Assert.Equal(PlaylistSongSeeds.PlaylistSongA.Song.SongUrl, detailModel.SongUrl);
        Assert.Equal(PlaylistSongSeeds.PlaylistSongA.Song.DurationInSeconds, detailModel.SongDurationInSeconds.TotalSeconds);
    } 
    
    [Fact]
    public void MapDetailToListModel_PlaylistSong_GetValues()
    {
        //Arrange
        var detailModel = PlaylistSongModelMapper.MapToDetailModel(PlaylistSongSeeds.PlaylistSongA);
        var listModel = PlaylistSongModelMapper.MapToListModel(detailModel);
        
        //Assert
        Assert.Equal(detailModel.Id, listModel.Id);
        Assert.Equal(detailModel.SongId, listModel.SongId);
        Assert.Equal(detailModel.SongName, listModel.SongName);
        Assert.Equal(detailModel.SongDurationInSeconds, listModel.SongDurationInSeconds);
    }

    [Fact]
    public void MapToEntity_PlaylistSong_Throws()
    {
        //Arrange
        var detailModel = PlaylistSongModelMapper.MapToDetailModel(PlaylistSongSeeds.PlaylistSongA);
        
        //Act & Assert
        Assert.Throws<NotImplementedException>(() =>
        {
            PlaylistSongModelMapper.MapToEntity(detailModel);
        });
    }
}