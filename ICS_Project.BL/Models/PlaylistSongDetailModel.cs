namespace ICS_Project.BL.Models;

public record PlaylistSongDetailModel : ModelBase
{
    public required Guid SongId { get; set; }
    public required string SongName { get; set; }
    public TimeSpan SongDurationInSeconds { get; set; }
    public required string Artist { get; set; }
    public required string Genre { get; set; }
    public required string SongUrl { get; set; }

    public static PlaylistSongDetailModel Empty => new()
    {
        Id = Guid.NewGuid(),
        SongId = Guid.Empty,
        SongName = string.Empty,
        SongDurationInSeconds = TimeSpan.Zero,
        Artist = string.Empty,
        Genre = string.Empty,
        SongUrl = string.Empty
    };
}