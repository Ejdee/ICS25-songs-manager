using System;
using ICS_Project.DAL.Entities;
using ICS_Project.DAL.Mappers;
using Xunit;
using Xunit.Abstractions;

namespace ICS_Project.DAL.Tests;

public class PlaylistSongEntityMapperTests(ITestOutputHelper output) : IntegrationTestBase(output)
{
    
    [Fact]
    public void MapToExistingEntity_UpdateFields()
    {
        // Arrange 
        
        var oldPlaylistId = Guid.Parse("00000000-0000-0000-0000-000000000001"); 
        var oldSongId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        var newPlaylistId = Guid.Parse("00000000-0000-0000-0000-000000000003");
        var newSongId = Guid.Parse("00000000-0000-0000-0000-000000000004");
        
        var existing = new PlaylistSongEntity
        {
            PlaylistId = oldPlaylistId,
            SongId = oldSongId,
            Song = new SongEntity 
            {
                Name = "Old Name",
                Description = "Old description",
                Artist = "Old Artist",
                DurationInSeconds = TimeSpan.FromSeconds(100),
                Genre = "Old Genre",
                SongUrl = "Old Song URL",
            },
            Playlist = new PlaylistEntity
            {
                Name = "Old Playlist Name",
                Description = "Old Playlist description",
            }
        };

        var newEntity = new PlaylistSongEntity
        {
            PlaylistId = newPlaylistId,
            SongId = newSongId,
            Song = new SongEntity 
            {
                Name = "New Name",
                Description = "New description",
                Artist = "New Artist",
                DurationInSeconds = TimeSpan.FromSeconds(200),
                Genre = "New Genre",
                SongUrl = "New Song URL",
            },
            Playlist = new PlaylistEntity
            {
                Name = "New Playlist Name",
                Description = "New Playlist description",
            }
        };

        var mapper = new PlaylistSongEntityMapper();

        // Act
        mapper.MapToExistingEntity(existing, newEntity);

        // Assert
        Assert.Equal(newPlaylistId, existing.PlaylistId);
        Assert.Equal(newSongId, existing.SongId);
    }
}