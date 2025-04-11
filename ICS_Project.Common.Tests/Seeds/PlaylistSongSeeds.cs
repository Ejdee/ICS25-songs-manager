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
    
    public static readonly PlaylistSongEntity PlaylistSongA = new()
    {
        Id = Guid.Parse("1bb8c8b0-39c3-4610-a0b7-23ba29584581"),
        PlaylistId = PlaylistSeeds.PlaylistA.Id,
        SongId = SongSeeds.SongA.Id,
        Playlist = PlaylistSeeds.PlaylistA,
        Song = SongSeeds.SongA
    };
    
    public static readonly PlaylistSongEntity PlaylistSongB = new()
    {
        Id = Guid.Parse("46aa3dd4-81d3-432f-83a4-74a2ae5ef9fa"),
        PlaylistId = PlaylistSeeds.PlaylistA.Id,
        SongId = SongSeeds.SongB.Id,
        Playlist = PlaylistSeeds.PlaylistA,
        Song = SongSeeds.SongB
    };
    
    public static readonly PlaylistSongEntity PlaylistSongC = new()
    {
        Id = Guid.Parse("93f81f43-a281-402d-8861-a3007cfb54ec"),
        PlaylistId = PlaylistSeeds.PlaylistA.Id,
        SongId = SongSeeds.SongC.Id,
        Playlist = PlaylistSeeds.PlaylistA,
        Song = SongSeeds.SongC
    };
    
    public static DbContext SeedPlaylistSongs(this DbContext dbx)
    {
        dbx.Set<PlaylistSongEntity>().AddRange(
            PopPlaylistSongs, 
            PopPlaylistSongs2,
            PopPlaylistSongsUpdate,
            PopPlaylistSongsDelete,
            PlaylistSongA,
            PlaylistSongB,
            PlaylistSongC
            );
        
        return dbx;
    }
}