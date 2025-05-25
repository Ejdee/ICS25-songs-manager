using System.ComponentModel;

namespace ICS_Project.BL.Models.Enums;

public enum SortOptions
{
    [Description("Song Name")]
    SongName,
    
    [Description("Song Duration")]
    SongDuration,
    
    [Description("Playlist Name")]
    PlaylistName,
    [Description("Song Count")]
    PlaylistSongCount,
    [Description("Playlist Duration")]
    PlaylistDuration,
}