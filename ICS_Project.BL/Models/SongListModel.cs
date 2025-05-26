namespace ICS_Project.BL.Models;

public record SongListModel : ModelBase
{
    public required string Name { get; set; }
    public string? SongUrl { get; set; }
    public string? Genre { get; set; }
    public TimeSpan DurationInSeconds { get; set; }

    public static SongListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Name = string.Empty,
        DurationInSeconds = TimeSpan.Zero,
        SongUrl = string.Empty,
        Genre = string.Empty
    };
}