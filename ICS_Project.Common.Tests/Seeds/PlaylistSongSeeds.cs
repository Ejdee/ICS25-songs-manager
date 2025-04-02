using ICS_Project.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS_Project.Common.Tests.Seeds;

public static class PlaylistSongSeeds
{
    public static readonly PlaylistSongEntity EmptyPlaylistSong = new()
    {
        Id = default,
        PlaylistId = default,
        SongId = default,
        Playlist = default!,
        Song = default!,
    };
    
    public static readonly PlaylistSongEntity PopPlaylistSongs = new()
    {
        Id = Guid.Parse("83c0017c-fb25-42f8-8c5c-a7589c462620"),
        PlaylistId = PlaylistSeeds.PopPlaylist.Id,
        SongId = SongSeeds.ShapeOfYou.Id,
        Playlist = PlaylistSeeds.PopPlaylist,
        Song = SongSeeds.ShapeOfYou
    };
    
    public static readonly PlaylistSongEntity PopPlaylistSongs2 = new()
    {
        Id = Guid.Parse("1935807e-4bd3-4dca-8b53-a798d7373c91"),
        PlaylistId = PlaylistSeeds.PopPlaylist.Id,
        SongId = SongSeeds.BillieJean.Id,
        Playlist = PlaylistSeeds.PopPlaylist,
        Song = SongSeeds.BillieJean
    };
    
    public static readonly PlaylistSongEntity PopPlaylistSongsUpdate = PopPlaylistSongs with { Id = Guid.Parse("8f7ac2e9-f87f-432e-8c0f-f0dac70fad03"), Playlist = null!, Song = null!, PlaylistId = PlaylistSeeds.PlaylistForSongsUpdate.Id};
    public static readonly PlaylistSongEntity PopPlaylistSongsDelete = PopPlaylistSongs with { Id = Guid.Parse("2e40c110-ec69-4fea-8317-693aaaa25265"), Playlist = null!, Song = null!, PlaylistId = PlaylistSeeds.PlaylistForSongsDelete.Id};
    
    public static DbContext SeedPlaylistSongs(this DbContext dbx)
    {
        dbx.Set<PlaylistSongEntity>().AddRange(
            PopPlaylistSongs, 
            PopPlaylistSongs2,
            PopPlaylistSongsUpdate,
            PopPlaylistSongsDelete
            );
        
        return dbx;
    }
}