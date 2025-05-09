using System;
using System.Threading.Tasks;
using ICS_Project.Common.Tests;
using ICS_Project.Common.Tests.Seeds;
using ICS_Project.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace ICS_Project.DAL.Tests;

[Collection("Sequential")]
public class SongTests(ITestOutputHelper output) : IntegrationTestBase(output)
{
    [Fact]
    public async Task AddNew_Song_Persisted()
    {
        // Arrange
        SongEntity entity = new()
        {
            Id = Guid.Parse("C5DE45D7-64A0-4E8D-AC7F-BF5CFDFB0EFC"),
            Name = "Test Song",
            Description = "Test Song Description",
            Genre = "Test Genre",
            DurationInSeconds = 300,
            Artist = "Test Artist",
            SongUrl = "https://example.com/test_song.mp3"
        };

        // Act
        IcsDbContextSut.Songs.Add(entity);
        await IcsDbContextSut.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Songs.SingleAsync(s => s.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task Update_Song_Persisted()
    {
        // Arrange
        var baseEntity = SongSeeds.SongEntityUpdate;
        var entity = baseEntity with
        {
            Name = baseEntity.Name + " Updated",
            Description = baseEntity.Description + " Updated"
        };

        // Act
        IcsDbContextSut.Songs.Update(entity);
        await IcsDbContextSut.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Songs.SingleAsync(s => s.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }
    
    [Fact]
    public async Task Delete_Song_SeededSongDeleted()
    {
        // Arrange
        var entityBase = SongSeeds.SongEntityDelete;

        // Act
        IcsDbContextSut.Songs.Remove(entityBase);
        await IcsDbContextSut.SaveChangesAsync();

        // Assert
        Assert.False(await IcsDbContextSut.Songs.AnyAsync(s => s.Id == entityBase.Id));
    }

    
    [Fact]
    public async Task DeleteById_Song_SeededSongDeleted()
    {
        // Arrange
        var entityBase = SongSeeds.SongEntityDelete;

        // Act
        IcsDbContextSut.Remove(await IcsDbContextSut.Songs.SingleAsync(s => s.Id == entityBase.Id));
        await IcsDbContextSut.SaveChangesAsync();

        // Assert
        Assert.False(await IcsDbContextSut.Songs.AnyAsync(s => s.Id == entityBase.Id));
    }

    [Fact]
    public async Task Delete_SongUsedInPlaylist_Throws()
    {
        // Arrange
        var entityBase = SongSeeds.ShapeOfYou;
        
        // Act & Assert
        IcsDbContextSut.Songs.Remove(entityBase);
        await Assert.ThrowsAsync<DbUpdateException>(async () => await IcsDbContextSut.SaveChangesAsync());
    }
}