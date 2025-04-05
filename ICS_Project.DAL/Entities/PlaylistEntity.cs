using ICS_Project.DAL.Interfaces;

namespace ICS_Project.DAL.Entities;

public record PlaylistEntity : IEntity
{
    public Guid Id { get; init; }
    public required string Name { get; set; }
    public ICollection<PlaylistSongEntity> PlaylistSongs { get; init; } = new List<PlaylistSongEntity>();
}