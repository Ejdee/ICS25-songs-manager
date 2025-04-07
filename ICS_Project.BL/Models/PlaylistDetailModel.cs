using System.Collections.ObjectModel;

namespace ICS_Project.BL.Models;

public record PlaylistDetailModel : ModelBase
{ 
    public required string Name { get; init; }
    public required string Description { get; init; }
    public TimeSpan DurationInSeconds { get; init; }
    public int SongCount { get; init; }
    public ObservableCollection<PlaylistSongListModel> Songs { get; init; } = new();
    
    public static PlaylistDetailModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Name = string.Empty,
        Description = String.Empty,
        DurationInSeconds = TimeSpan.Zero,
        SongCount = 0
    };
}