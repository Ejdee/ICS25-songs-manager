using System;
using ICS_Project.DAL.Entities;
using ICS_Project.DAL.Mappers;
using Xunit;
using Xunit.Abstractions;

namespace ICS_Project.DAL.Tests;

public class SongEntityMapperTests(ITestOutputHelper output) : IntegrationTestBase(output)
{
    [Fact]
    public void MapToExistingEntity_UpdateFields()
    {
        var existing = new SongEntity
        {
            Name = "Old Name",
            Description = "Old description",
            Artist = "Old Artist",
            DurationInSeconds = TimeSpan.FromSeconds(100),
            Genre = "Old Genre",
            SongUrl = "Old Song URL",
        };
       
        var newEntity = new SongEntity
        {
            Name = "New Name",
            Description = "New Description",
            Artist = "New Artist",
            DurationInSeconds = TimeSpan.FromSeconds(200),
            Genre = "New Genre",
            SongUrl = "New Song URL",
        };

        var mapper = new SongEntityMapper();
        
        mapper.MapToExistingEntity(existing, newEntity);
        
        Assert.Equal("New Name", existing.Name);
        Assert.Equal("New Description", existing.Description);
        Assert.Equal("New Artist", existing.Artist);
        Assert.Equal(TimeSpan.FromSeconds(200), existing.DurationInSeconds);
        Assert.Equal("New Genre", existing.Genre);
        Assert.Equal("New Song URL", existing.SongUrl);
    }
}