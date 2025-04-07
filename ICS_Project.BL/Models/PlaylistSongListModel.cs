namespace ICS_Project.BL.Models;

public record PlaylistSongListModel : ModelBase 
{
    public required Guid SongId { get; set; }
    public required string SongName { get; set; }
    public TimeSpan SongDurationInSeconds { get; set; }
    
    public static PlaylistSongListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        SongId = Guid.Empty,
        SongName = string.Empty,
        SongDurationInSeconds = TimeSpan.Zero,
    };
}