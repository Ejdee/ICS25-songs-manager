using ICS_Project.DAL.Entities;
using ICS_Project.DAL.Mappers;
using Xunit;
using Xunit.Abstractions;

namespace ICS_Project.DAL.Tests;

public class PlaylistEntityMapperTests(ITestOutputHelper output) : IntegrationTestBase(output)
{
    [Fact]
    public void MapToExistingEntity_UpdateFields()
    {
        var existing = new PlaylistEntity
        {
            Name = "Old Name",
            Description = "Old description",
        };

        var newEntity = new PlaylistEntity
        {
            Name = "New Name",
            Description = "New description",
        };

        var mapper = new PlaylistEntityMapper();

        mapper.MapToExistingEntity(existing, newEntity);

        Assert.Equal("New Name", existing.Name);
        Assert.Equal("New description", existing.Description);
    }
}