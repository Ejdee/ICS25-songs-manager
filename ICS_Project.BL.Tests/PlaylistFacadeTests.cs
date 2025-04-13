using System.Collections.ObjectModel;
using ICS_Project.BL.Facades;
using ICS_Project.BL.Models;
using ICS_Project.Common.Tests;
using ICS_Project.BL.Facades.Interfaces;
using ICS_Project.BL.Models.Enums;
using ICS_Project.Common.Tests.Seeds;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ICS_Project.BL.Tests;

public class PlaylistFacadeTests : FacadeTestBase
{
    private readonly IPlaylistFacade _facadeSUT;

    public PlaylistFacadeTests(ITestOutputHelper output) : base(output)
    {
        _facadeSUT = new PlaylistFacade(UnitOfWorkFactory, PlaylistModelMapper);
    }

    [Fact]
    public async Task Create_WithoutSongs_EqualsCreated()
    {
        //Arrange
        var model = new PlaylistDetailModel()
        {
            Name = "Playlist 1",
            Description = "Testing playlist 1",
            SongCount = 0,
            DurationInSeconds = TimeSpan.Zero,
        };

        //Act
        var returnedModel = await _facadeSUT.SaveAsync(model);

        //Assert
        FixIds(model, returnedModel);
        DeepAssert.Equal(model, returnedModel);
    }

    [Fact]
    public async Task Create_WithNonExistingSong_Throws()
    {
        //Arrange
        var model = new PlaylistDetailModel()
        {
            Name = "Playlist 1",
            Description = "Testing playlist 1",
            SongCount = 0,
            DurationInSeconds = TimeSpan.Zero,
            Songs = new ObservableCollection<PlaylistSongListModel>
            {
                new ()
                {
                    Id = Guid.Empty,
                    SongId = Guid.Empty,
                    SongName = "Non-existing song",
                    SongDurationInSeconds = TimeSpan.FromSeconds(0),
                }
            }
        };
        
        //Act & Assert
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _facadeSUT.SaveAsync(model));
    }
    
    [Fact]
    public async Task Create_WithExistingSong_Throws()
    {
        //Arrange
        var model = new PlaylistDetailModel()
        {
            Name = "Playlist 1",
            Description = "Testing playlist 1",
            SongCount = 1,
            DurationInSeconds = TimeSpan.FromSeconds(0),
            Songs = new ObservableCollection<PlaylistSongListModel>
            {
                new ()
                {
                    SongId = SongSeeds.ShapeOfYou.Id,
                    SongName = SongSeeds.ShapeOfYou.Name,
                    SongDurationInSeconds = TimeSpan.FromSeconds(SongSeeds.ShapeOfYou.DurationInSeconds),
                }
            }
        };

        //Act & Assert
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _facadeSUT.SaveAsync(model));
    }

    [Fact]
    public async Task Create_WithExistingAndNonExistingSong_Throws()
    {
        //Arrange
        var model = new PlaylistDetailModel()
        {
            Name = "Playlist 1",
            Description = "Testing playlist 1",
            SongCount = 1,
            DurationInSeconds = TimeSpan.FromSeconds(0),
            Songs = new ObservableCollection<PlaylistSongListModel>
            {
                new ()
                {
                    Id = Guid.Empty,
                    SongId = Guid.Empty,
                    SongName = "Non-existing song",
                    SongDurationInSeconds = TimeSpan.FromSeconds(0),
                },
                PlaylistSongModelMapper.MapToListModel(PlaylistSongSeeds.PopPlaylistSongs)
            }
        };
        
        //Act & Assert
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _facadeSUT.SaveAsync(model));
    }

    [Fact]
    public async Task GetById_FromSeeded_ContainsSeeded()
    {
        //Arrange
        var detailModel = PlaylistModelMapper.MapToDetailModel(PlaylistSeeds.PopPlaylist);
        
        //Act
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);
        
        //Assert
        DeepAssert.Equal(detailModel, returnedModel);
    }
    
    [Fact]
    public async Task GetById_NonExistent_ReturnsNull()
    {
        //Arrange
        var detailModel = PlaylistModelMapper.MapToDetailModel(PlaylistSeeds.EmptyPlaylist);
        
        //Act
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);
        
        //Assert
        Assert.Null(returnedModel);
    }

    [Fact]
    public async Task GetAll_FromSeeded_ContainsSeeded()
    {
        //Arrange
        var listModel = PlaylistModelMapper.MapToListModel(PlaylistSeeds.PopPlaylist);
        
        //Act
        var returnedModel = await _facadeSUT.GetAsync();
        
        //Assert
        Assert.Contains(listModel, returnedModel);
    }

    [Fact]
    public async Task Update_FromSeeded_DoesNotThrow()
    {
        //Arrange
        var detailModel = PlaylistModelMapper.MapToDetailModel(PlaylistSeeds.PopPlaylist);
        detailModel.Name = "Different name";

        //Act & Assert
        await _facadeSUT.SaveAsync(detailModel with { Songs = default! });
    }

    [Fact]
    public async Task Update_Name_FromSeeded_Updated()
    {
        //Arrange
        var detailModel = PlaylistModelMapper.MapToDetailModel(PlaylistSeeds.PopPlaylist);
        detailModel.Name = "Different name"; 
        
        //Act
        await _facadeSUT.SaveAsync(detailModel with { Songs = default! });
        
        //Assert
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal(detailModel, returnedModel);
    }

    [Fact]
    public async Task Update_RemovePlaylist_FromSeeded_NotUpdated()
    {
        //Arrange
        var detailModel = PlaylistModelMapper.MapToDetailModel(PlaylistSeeds.PopPlaylist);
        detailModel.Songs.Clear();
        
        //Act
        await _facadeSUT.SaveAsync(detailModel);
        
        //Assert
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal(PlaylistModelMapper.MapToDetailModel(PlaylistSeeds.PopPlaylist), returnedModel);
    }

    [Fact]
    public async Task Update_RemoveOneOfSongs_FromSeeded_Updated()
    {
        //Arrange
        var detailModel = PlaylistModelMapper.MapToDetailModel(PlaylistSeeds.PopPlaylist);
        detailModel.Songs.RemoveAt(0);
        
        //Act
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _facadeSUT.SaveAsync(detailModel));
        
        //Assert
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal(PlaylistModelMapper.MapToDetailModel(PlaylistSeeds.PopPlaylist), returnedModel);
    }
    
    [Fact]
    public async Task DeleteById_FromSeeded_DoesNotThrow()
    {
        //Act & Assert
        await _facadeSUT.DeleteAsync(PlaylistSeeds.PopPlaylist.Id);
    }

    [Fact]
    public async Task SearchByNameAsync_ReturnCorrectPlaylistUniqueName()
    {
        //Arrange
        var model = new PlaylistDetailModel()
        {
            Name = "PlaylistToBeSearchedUniqueName",
            Description = "Testing playlist 1",
            SongCount = 0,
            DurationInSeconds = TimeSpan.Zero,
        };
        
        //Act
        var returnedModel = await _facadeSUT.SaveAsync(model);
        var result = await _facadeSUT.SearchByNameAsync("PlaylistToBeSearchedUniqueName");

        var resultList = result.ToList(); 
        
        //Assert
        Assert.NotNull(result);
        Assert.Single(resultList);
        Assert.Equal(returnedModel.Id, resultList.First().Id);
    }
    
    [Fact]
    public async Task SearchByNameAsync_ReturnCorrectPlaylistsFromSeeds()
    {
        //Act
        var result = await _facadeSUT.SearchByNameAsync("Pop");

        //Assert
        Assert.NotNull(result);
        Assert.Equal(6, result.Count());
    }
    
    [Fact]
    public async Task SearchByNameAsync_ReturnNonExistingPlaylist()
    {
        //Act
        var result = await _facadeSUT.SearchByNameAsync("Non-existing playlist");

        //Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetSortedAsync_ByNameAscending()
    {
        //Act
        var result = await _facadeSUT.GetSortedAsync(PlaylistSortOption.Name, true);
        var resultList = result.ToList();
        
        //Assert
        Assert.NotNull(result);
        var sorted = resultList.OrderBy(p => p.Name).ToList();
        Assert.StartsWith("A", resultList.First().Name);
        Assert.Equal(sorted, resultList);
    }

    [Fact]
    public async Task GetSortedAsync_ByNameDescending()
    {
        //Act
        var result = await _facadeSUT.GetSortedAsync(PlaylistSortOption.Name, false);
        var resultList = result.ToList();
        
        //Assert
        Assert.NotNull(result);
        var sorted = resultList.OrderByDescending(p => p.Name).ToList();
        Assert.StartsWith("Z", resultList.First().Name);
        Assert.Equal(sorted, resultList);
    }
    
    [Fact]
    public async Task GetSortedAsync_BySongCountAscending()
    {
        //Act
        var result = await _facadeSUT.GetSortedAsync(PlaylistSortOption.SongCount, true);
        var resultList = result.ToList();
        
        //Assert
        Assert.NotNull(result);
        var sorted = resultList.OrderBy(p => p.SongCount).ToList();
        Assert.Equal(sorted, resultList);
        Assert.True(resultList.First().SongCount <= resultList.Last().SongCount);
    }

    [Fact]
    public async Task GetSortedAsync_BySongCountDescending()
    {
        //Act
        var result = await _facadeSUT.GetSortedAsync(PlaylistSortOption.SongCount, false);
        var resultList = result.ToList();
        
        //Assert
        Assert.NotNull(result);
        var sorted = resultList.OrderByDescending(p => p.SongCount).ToList();
        Assert.Equal(sorted, resultList);
        Assert.True(resultList.First().SongCount >= resultList.Last().SongCount);
    }
    
    [Fact]
    public async Task GetSortedAsync_ByDurationAscending()
    {
        //Act
        var result = await _facadeSUT.GetSortedAsync(PlaylistSortOption.Duration, true);
        var resultList = result.ToList();
        
        //Assert
        Assert.NotNull(result);
        var sorted = resultList.OrderBy(p => p.DurationInSeconds).ToList();
        Assert.Equal(sorted, resultList);
        Assert.True(resultList.First().DurationInSeconds <= resultList.Last().DurationInSeconds);
    }

    [Fact]
    public async Task GetSortedAsync_ByDurationDescending()
    {
        //Act
        var result = await _facadeSUT.GetSortedAsync(PlaylistSortOption.Duration, false);
        var resultList = result.ToList();
        
        //Assert
        Assert.NotNull(result);
        var sorted = resultList.OrderByDescending(p => p.DurationInSeconds).ToList();
        Assert.Equal(sorted, resultList);
        Assert.True(resultList.First().DurationInSeconds >= resultList.Last().DurationInSeconds);
    }
    
    [Fact]
    public async Task GetSortedAsync_InvalidSortOption_Throws()
    {
        //Act & Assert
        await Assert.ThrowsAnyAsync<ArgumentOutOfRangeException>(() => _facadeSUT.GetSortedAsync((PlaylistSortOption)100));
    }
    
    private static void FixIds(PlaylistDetailModel expectedModel, PlaylistDetailModel returnedModel)
    {
        returnedModel.Id = expectedModel.Id;
        
        foreach (var playlistSongModel in returnedModel.Songs)
        {
            var playlistSongDetailModel = expectedModel.Songs
                .FirstOrDefault(s => 
                    s.SongName == playlistSongModel.SongName &&
                    s.SongDurationInSeconds == playlistSongModel.SongDurationInSeconds);

            if (playlistSongDetailModel != null)
            {
                playlistSongModel.Id = playlistSongDetailModel.Id;
                playlistSongModel.SongId = playlistSongDetailModel.SongId;
            }
        }
    }
}