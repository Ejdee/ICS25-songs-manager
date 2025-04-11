using System.Collections.ObjectModel;

namespace ICS_Project.BL.Models;

public record PlaylistDetailModel : ModelBase
{ 
    public required string Name { get; set; }
    public required string Description { get; set; }
    public TimeSpan DurationInSeconds { get; set; }
    public int SongCount { get; set; }
    public ObservableCollection<PlaylistSongListModel> Songs { get; init; } = new();
    
    public static PlaylistDetailModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Name = string.Empty,
        Description = string.Empty,
        DurationInSeconds = TimeSpan.Zero,
        SongCount = 0
    };
}