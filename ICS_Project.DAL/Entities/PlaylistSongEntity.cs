using ICS_Project.DAL.Interfaces;

namespace ICS_Project.DAL.Entities;

public record PlaylistSongEntity : IEntity
{
    public required Guid PlaylistId { get; set; }
    public required Guid SongId { get; set; }
    
    public required PlaylistEntity Playlist { get; init; }
    public required SongEntity Song { get; init; }
    
    public Guid Id { get; init; }
}