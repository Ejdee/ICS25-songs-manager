using ICS_Project.BL.Mappers;
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
    public void MapToListModel_PlaylistSong_NullInput_ReturnsEmpty()
    {
        //Act
        var listModel = PlaylistSongModelMapper.MapToListModel((PlaylistSongEntity)null!);
        
        //Assert
        Assert.Equal(string.Empty, listModel.SongName);
        Assert.Equal(TimeSpan.Zero, listModel.SongDurationInSeconds);
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
    
    [Fact]
    public void MapToDetailModel_Song_GetValues()
    {
        //Arrange
        var song = SongSeeds.BillieJean;
    
        //Act
        var detailModel = SongModelMapper.MapToDetailModel(song);
    
        //Assert
        Assert.Equal(song.Id, detailModel.Id);
        Assert.Equal(song.Name, detailModel.Name);
        Assert.Equal(song.Description, detailModel.Description);
        Assert.Equal(song.DurationInSeconds, detailModel.DurationInSeconds.TotalSeconds);
        Assert.Equal(song.Genre, detailModel.Genre);
        Assert.Equal(song.Artist, detailModel.Artist);
        Assert.Equal(song.SongUrl, detailModel.SongUrl);
    }
    
    [Fact]
    public void MapToDetailModel_Song_NullInput_ReturnsEmpty()
    {
        //Act
        var detailModel = SongModelMapper.MapToDetailModel(null);
    
        //Assert
        Assert.Equal(string.Empty, detailModel.Name);
        Assert.Equal(TimeSpan.Zero, detailModel.DurationInSeconds);
        Assert.Equal(string.Empty, detailModel.Description);
        Assert.Equal(string.Empty, detailModel.Genre);
        Assert.Equal(string.Empty, detailModel.Artist);
        Assert.Equal(string.Empty, detailModel.SongUrl);
    }
    
    [Fact]
    public void MapToListModel_Song_NullInput_ReturnsEmpty()
    {
        //Act
        var listModel = SongModelMapper.MapToListModel((SongEntity)null!);
    
        //Assert
        Assert.Equal(string.Empty, listModel.Name);
        Assert.Equal(TimeSpan.Zero, listModel.DurationInSeconds);
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
    public void MapToListModel_Song_GetValues()
    {
        //Arrange
        var song = SongSeeds.ShapeOfYou;
    
        //Act
        var listModel = SongModelMapper.MapToListModel(song);
    
        //Assert
        Assert.Equal(song.Id, listModel.Id);
        Assert.Equal(song.Name, listModel.Name);
        Assert.Equal(song.DurationInSeconds, listModel.DurationInSeconds.TotalSeconds);
    }

    [Fact]
    public void MapDetailToListModel_Song_GetValues()
    {
        //Arrange
        var detailModel = SongModelMapper.MapToDetailModel(SongSeeds.BillieJean);
    
        //Act
        var listModel = SongModelMapper.MapToListModel(SongModelMapper.MapToEntity(detailModel));
    
        //Assert
        Assert.Equal(detailModel.Id, listModel.Id);
        Assert.Equal(detailModel.Name, listModel.Name);
        Assert.Equal(detailModel.DurationInSeconds, listModel.DurationInSeconds);
    }
    
    [Fact]
    public void MapToEntity_Song_GetValues()
    {
        //Arrange
        var detailModel = SongModelMapper.MapToDetailModel(SongSeeds.ShapeOfYou);
    
        //Act
        var entity = SongModelMapper.MapToEntity(detailModel);
    
        //Assert
        Assert.Equal(detailModel.Id, entity.Id);
        Assert.Equal(detailModel.Name, entity.Name);
        Assert.Equal(detailModel.Description, entity.Description);
        Assert.Equal((int)detailModel.DurationInSeconds.TotalSeconds, entity.DurationInSeconds);
        Assert.Equal(detailModel.Genre, entity.Genre);
        Assert.Equal(detailModel.Artist, entity.Artist);
        Assert.Equal(detailModel.SongUrl, entity.SongUrl);
    }
    
    [Fact]
    public void MapToListModel_Playlist_GetValues()
    {
        //Arrange
        var playlist = PlaylistSeeds.PlaylistA;
        var mapper = new PlaylistModelMapper(new PlaylistSongModelMapper());
        
        //Act
        var listModel = mapper.MapToListModel(playlist);
        
        //Assert
        Assert.Equal(playlist.Id, listModel.Id);
        Assert.Equal(playlist.Name, listModel.Name);
        Assert.Equal(TimeSpan.FromSeconds(playlist.PlaylistSongs.Sum(x => x.Song.DurationInSeconds)), listModel.DurationInSeconds);
        Assert.Equal(playlist.PlaylistSongs.Count, listModel.SongCount);
    }
    
    [Fact]
    public void MapToListModel_Playlist_NullInput_ReturnsEmpty()
    {
        //Arrange
        var mapper = new PlaylistModelMapper(new PlaylistSongModelMapper());
        
        //Act
        var listModel = mapper.MapToListModel((PlaylistEntity)null!);
        
        //Assert
        Assert.Equal(string.Empty, listModel.Name);
        Assert.Equal(TimeSpan.Zero, listModel.DurationInSeconds);
        Assert.Equal(0, listModel.SongCount);
    }
    
    [Fact]
    public void MapToDetailModel_Playlist_GetValues()
    {
        //Arrange
        var playlist = PlaylistSeeds.PlaylistA;
        var mapper = new PlaylistModelMapper(new PlaylistSongModelMapper());
        
        //Act
        var detailModel = mapper.MapToDetailModel(playlist);
        
        //Assert
        Assert.Equal(playlist.Id, detailModel.Id);
        Assert.Equal(playlist.Name, detailModel.Name);
        Assert.Equal(playlist.Description, detailModel.Description);
        Assert.Equal(TimeSpan.FromSeconds(playlist.PlaylistSongs.Sum(x => x.Song.DurationInSeconds)), detailModel.DurationInSeconds);
        Assert.Equal(playlist.PlaylistSongs.Count, detailModel.SongCount);
        Assert.NotNull(detailModel.Songs);
        Assert.Equal(playlist.PlaylistSongs.Count, detailModel.Songs.Count);
    }
    
    [Fact]
    public void MapToDetailModel_Playlist_NullInput_ReturnsEmpty()
    {
        //Arrange
        var mapper = new PlaylistModelMapper(new PlaylistSongModelMapper());
        
        //Act
        var detailModel = mapper.MapToDetailModel(null);
        
        //Assert
        Assert.Equal(string.Empty, detailModel.Name);
        Assert.Equal(string.Empty, detailModel.Description);
        Assert.Equal(TimeSpan.Zero, detailModel.DurationInSeconds);
        Assert.Equal(0, detailModel.SongCount);
        Assert.NotNull(detailModel.Songs);
        Assert.Empty(detailModel.Songs);
    }
    
    [Fact]
    public void MapToEntity_Playlist_GetValues()
    {
        //Arrange
        var playlist = PlaylistSeeds.PlaylistA;
        var mapper = new PlaylistModelMapper(new PlaylistSongModelMapper());
        var detailModel = mapper.MapToDetailModel(playlist);
        
        //Act
        var entity = mapper.MapToEntity(detailModel);
        
        //Assert
        Assert.Equal(detailModel.Id, entity.Id);
        Assert.Equal(detailModel.Name, entity.Name);
        Assert.Equal(detailModel.Description, entity.Description);
        // Note: PlaylistSongs are not mapped in MapToEntity
    }
    
    [Fact]
    public void MapToListModels_Playlists_IEnumerable()
    {
        //Arrange
        var playlists = new List<PlaylistEntity>
        {
            PlaylistSeeds.PlaylistA,
            PlaylistSeeds.PlaylistB
        };
        var mapper = new PlaylistModelMapper(new PlaylistSongModelMapper());
        
        //Act
        var mappedList = mapper.MapToListModel(playlists);
        
        //Assert
        Assert.NotNull(mappedList);
        Assert.Equal(playlists.Count, mappedList.Count());
    }
}