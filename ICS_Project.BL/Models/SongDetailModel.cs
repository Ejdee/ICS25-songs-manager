namespace ICS_Project.BL.Models;

public record SongDetailModel : ModelBase
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public TimeSpan DurationInSeconds { get; set; }
    public required string Artist { get; set; }
    public required string Genre { get; set; }
    public required string SongUrl { get; set; }

    public static SongDetailModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Description = string.Empty,
        Name = string.Empty,
        DurationInSeconds = TimeSpan.Zero,
        Artist = string.Empty,
        Genre = string.Empty,
        SongUrl = string.Empty
    };
    
}