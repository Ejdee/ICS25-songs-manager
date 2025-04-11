using ICS_Project.DAL.Interfaces;

namespace ICS_Project.DAL.Entities;

public record SongEntity: IEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Genre { get; set; }
    public int DurationInSeconds { get; set; }
    public required string Artist { get; set; }
    public required string SongUrl { get; set; }
}