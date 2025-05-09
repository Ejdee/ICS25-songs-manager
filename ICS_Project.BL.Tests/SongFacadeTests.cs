using ICS_Project.BL.Facades;
using ICS_Project.BL.Facades.Interfaces;
using ICS_Project.BL.Models;
using ICS_Project.BL.Models.Enums;
using ICS_Project.Common.Tests;
using ICS_Project.Common.Tests.Seeds;
using Xunit.Abstractions;

namespace ICS_Project.BL.Tests;

public class SongFacadeTests : FacadeTestBase
{
    private readonly ISongFacade _songFacadeSUT;
    
    public SongFacadeTests(ITestOutputHelper output) : base(output)
    {
        _songFacadeSUT = new SongFacade(UnitOfWorkFactory, SongModelMapper);
    }
    
    [Fact]
    public async Task Create_WithNonExistingSong_ShouldCreateSong()
    {
        //Arrange
        var newSong = new SongDetailModel
        {
            Id = Guid.NewGuid(),
            Description = "A new song",
            Name = "New Song",
            Genre = "Pop",
            DurationInSeconds = TimeSpan.FromSeconds(250),
            Artist = "new artist",
            SongUrl = "someurl"
        };

        //Act
        var createdSong = await _songFacadeSUT.SaveAsync(newSong);

        //Assert
        Assert.NotNull(createdSong);
        Assert.Equal(newSong.Name, createdSong.Name);
    }

    [Fact]
    public async Task GetAll_Single_BillieJeanSeeded()
    {
        //Arrange
        var songs = await _songFacadeSUT.GetAsync();
        var song = songs.Single(s => s.Id == SongSeeds.BillieJean.Id);
        
        //Act & Assert
        Assert.NotNull(song);
        DeepAssert.Equal(SongModelMapper.MapToListModel(SongSeeds.BillieJean), song);
    }

    [Fact]
    public async Task GetById_BillieJeanSeeded()
    {
        //Arrange
        var song = _songFacadeSUT.GetAsync(SongSeeds.BillieJean.Id);
        
        //Act & Assert
        Assert.NotNull(song);
        DeepAssert.Equal(SongModelMapper.MapToDetailModel(SongSeeds.BillieJean), await song);
    }

    [Fact]
    public async Task GetById_NonExistent()
    {
        //Arrange
        var song = await _songFacadeSUT.GetAsync(SongSeeds.EmptySongEntity.Id);
        
        //Act & Assert
        Assert.Null(song);
    }

    [Fact]
    public async Task DeleteById_SongSeeded()
    {
        //Arrange
        await _songFacadeSUT.DeleteAsync(SongSeeds.SongEntityDelete.Id);
        
        //Act
        var fetchedSong = await _songFacadeSUT.GetAsync(SongSeeds.SongEntityDelete.Id);
        
        //Assert
        Assert.Null(fetchedSong);
    }

    [Fact]
    public async Task Delete_SongUsedInPlaylist_Throws()
    {
        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _songFacadeSUT.DeleteAsync(SongSeeds.BillieJean.Id);
        });
    }

    [Fact]
    public async Task Insert_ExistingSong_Update()
    {
        //Arrange
        var existingSong = new SongDetailModel
        {
            Id = SongSeeds.BillieJean.Id,
            Description = SongSeeds.BillieJean.Description,
            Name = SongSeeds.BillieJean.Name,
            Genre = SongSeeds.BillieJean.Genre,
            DurationInSeconds = TimeSpan.FromSeconds(SongSeeds.BillieJean.DurationInSeconds),
            Artist = SongSeeds.BillieJean.Artist,
            SongUrl = SongSeeds.BillieJean.SongUrl,
        };
        
        existingSong.Name += "updated"; 
        existingSong.Description += "updated";
        
        //Act
        var updatedSong = await _songFacadeSUT.SaveAsync(existingSong);
        
        //Assert
        Assert.NotNull(updatedSong);
        DeepAssert.Equal(existingSong, updatedSong);
        
        var songFromDb = await _songFacadeSUT.GetAsync(SongSeeds.BillieJean.Id);
        Assert.NotNull(songFromDb);
        DeepAssert.Equal(existingSong, songFromDb);
    }

    [Fact]
    public async Task GetSortedAsync_ByNameAscending()
    {
        //Act
        var songs = await _songFacadeSUT.GetSortedAsync(SortOptions.SongName, true);
        var songList = songs.ToList();
        
        //Assert
        Assert.NotNull(songList);
        var sorted = songList.OrderBy(s => s.Name).ToList();
        Assert.StartsWith("A", songList.First().Name);
        Assert.Equal(sorted, songList);
    }
    
    [Fact]
    public async Task GetSortedAsync_ByNameDescending()
    {
        //Act
        var songs = await _songFacadeSUT.GetSortedAsync(SortOptions.SongName, false);
        var songList = songs.ToList();
        
        //Assert
        Assert.NotNull(songList);
        var sorted = songList.OrderByDescending(s => s.Name).ToList();
        Assert.StartsWith("Z", songList.First().Name);
        Assert.Equal(sorted, songList);
    }
    
    [Fact]
    public async Task GetSortedAsync_ByDurationAscending()
    {
        //Act
        var songs = await _songFacadeSUT.GetSortedAsync(SortOptions.SongDuration, true);
        var songList = songs.ToList();
        
        //Assert
        Assert.NotNull(songList);
        var sorted = songList.OrderBy(s => s.DurationInSeconds).ToList();
        Assert.Equal(sorted, songList);
        Assert.True(songList.First().DurationInSeconds <= songList.Last().DurationInSeconds);
    }
    
    [Fact]
    public async Task GetSortedAsync_ByDurationDescending()
    {
        //Act
        var songs = await _songFacadeSUT.GetSortedAsync(SortOptions.SongDuration, false);
        var songList = songs.ToList();
        
        //Assert
        Assert.NotNull(songList);
        var sorted = songList.OrderByDescending(s => s.DurationInSeconds).ToList();
        Assert.Equal(sorted, songList);
        Assert.True(songList.First().DurationInSeconds >= songList.Last().DurationInSeconds);
    }

    [Fact]
    public async Task SearchByName_SongSeeded()
    {
        //Act
        var song = await _songFacadeSUT.SearchByNameAsync(SongSeeds.SongZ.Name);
        var songList = song.ToList();
        
        //Assert 
        Assert.True(songList.All(s => s.Name == SongSeeds.SongZ.Name));
    }

    [Fact]
    public async Task SearchByName_UniqueName()
    {
        //Arrange
        var model = new SongDetailModel
        {
            Id = Guid.NewGuid(),
            Name = "UniqueSongName",
            Description = "A unique song",
            Genre = "Pop",
            DurationInSeconds = TimeSpan.FromSeconds(250),
            Artist = "new artist",
            SongUrl = "someurl"
        };
        
        //Act
        var createdSong = await _songFacadeSUT.SaveAsync(model);
        var song = await _songFacadeSUT.SearchByNameAsync(model.Name);
        var songList = song.ToList();
        
        //Assert
        Assert.NotNull(createdSong);
        Assert.NotNull(songList);
        Assert.Single(songList);
        Assert.Equal(model.Name, songList.First().Name);
    }
    
    [Fact]
    public async Task SearchByName_NonExistent()
    {
        //Act
        var song = await _songFacadeSUT.SearchByNameAsync("NonExistentSong");
        
        //Assert
        Assert.Empty(song);
    }
    
    [Fact]
    public async Task FilterByGenre_ReturnsOnlyMatchingGenre()
    {
        // Arrange
        var testGenre = "testGenre"; 
        
        var song1 = new SongDetailModel
        {
            Id = Guid.NewGuid(),
            Name = "Song1",
            Description = "A test song",
            Genre = testGenre,
            DurationInSeconds = TimeSpan.FromSeconds(180),
            Artist = "Artist1",
            SongUrl = "url1"
        };
    
        var song2 = new SongDetailModel
        {
            Id = Guid.NewGuid(),
            Name = "Song2",
            Description = "Another test song",
            Genre = "DifferentGenre",
            DurationInSeconds = TimeSpan.FromSeconds(200),
            Artist = "Artist2",
            SongUrl = "url2"
        };
    
        // Act
        var savedSong1 = await _songFacadeSUT.SaveAsync(song1);
        var savedSong2 = await _songFacadeSUT.SaveAsync(song2);
        var result = await _songFacadeSUT.FilterByGenreAsync(testGenre);
    
        // Assert
        var resultList = result.ToList();
        Assert.Contains(resultList, s => s.Id == savedSong1.Id);
        Assert.DoesNotContain(resultList, s => s.Id == savedSong2.Id);
    }
    
    [Fact]
    public async Task FilterByGenre_NonExistent()
    {
        //Act
        var song = await _songFacadeSUT.FilterByGenreAsync("NonExistentGenre");
        
        //Assert
        Assert.Empty(song);
    }
}