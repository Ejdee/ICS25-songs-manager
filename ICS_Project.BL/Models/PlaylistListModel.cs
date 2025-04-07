namespace ICS_Project.BL.Models;

public record PlaylistListModel : ModelBase
{
    public required string Name { get; set; }
    public TimeSpan DurationInSeconds { get; set; }
    public int SongCount { get; set; }

    public static PlaylistListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Name = string.Empty,
        DurationInSeconds = TimeSpan.Zero,
        SongCount = 0
    };
}