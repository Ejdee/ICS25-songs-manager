using ICS_Project.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS_Project.Common.Tests.Seeds;

public static class PlaylistSeeds
{
    public static readonly PlaylistEntity EmptyPlaylist = new()
    {
        Id = default,
        Name = default!,
        Description = default!,
    };
    
    public static readonly PlaylistEntity PopPlaylist = new()
    {
        Id = Guid.Parse("3166a30b-36d0-430a-90cd-bed126670147"),
        Name = "Pop",
        Description = "My favorite pop songs",
    };
    
    public static readonly PlaylistEntity PopPlaylistWithNoSongs = PopPlaylist with { Id = Guid.Parse("1244a30b-36d0-430a-90cd-bed126670148"), PlaylistSongs = new List<PlaylistSongEntity>()};
    public static readonly PlaylistEntity PopPlaylistUpdate = PopPlaylist with { Id = Guid.Parse("0999a30b-36d0-430a-90cd-bed126670148"), PlaylistSongs = new List<PlaylistSongEntity>()};
    public static readonly PlaylistEntity PopPlaylistDelete = PopPlaylist with { Id = Guid.Parse("8888a30b-36d0-430a-90cd-bed126670148"), PlaylistSongs = new List<PlaylistSongEntity>()};

    public static readonly PlaylistEntity PlaylistForSongsUpdate = PopPlaylist with { Id = Guid.Parse("7f012cf4-cccd-4371-842c-0102f58d4c17"), PlaylistSongs = new List<PlaylistSongEntity>()};
    public static readonly PlaylistEntity PlaylistForSongsDelete = PopPlaylist with { Id = Guid.Parse("00012152-cccd-4371-842c-0102f58d4c17"), PlaylistSongs = new List<PlaylistSongEntity>()};

    public static readonly PlaylistEntity PlaylistA = new()
    {
        Name = "A",
        Description = "Alphabetical sort test 1",
    };

    public static readonly PlaylistEntity PlaylistB = new()
    {
        Name = "B",
        Description = "Alphabetical sort test 2",
    };

    public static readonly PlaylistEntity PlaylistC = new()
    {
        Name = "C",
        Description = "Alphabetical sort test 3",
    }; 
    
    public static readonly PlaylistEntity PlaylistZ = new()
    {
        Name = "Z",
        Description = "Alphabetical sort test 4",
    }; 
    
    static PlaylistSeeds()
    {
        PopPlaylist.PlaylistSongs.Add(PlaylistSongSeeds.PopPlaylistSongs);
        PopPlaylist.PlaylistSongs.Add(PlaylistSongSeeds.PopPlaylistSongs2);
        
        PlaylistForSongsDelete.PlaylistSongs.Add(PlaylistSongSeeds.PopPlaylistSongsDelete);
        
        PlaylistA.PlaylistSongs.Add(PlaylistSongSeeds.PlaylistSongA);
        PlaylistA.PlaylistSongs.Add(PlaylistSongSeeds.PlaylistSongB);
        PlaylistA.PlaylistSongs.Add(PlaylistSongSeeds.PlaylistSongC);
    }
    
    public static DbContext SeedPlaylists(this DbContext dbx)
    {
        dbx.Set<PlaylistEntity>().AddRange(
            PopPlaylist,
            PopPlaylistWithNoSongs,
            PopPlaylistUpdate,
            PopPlaylistDelete,
            PlaylistForSongsUpdate,
            PlaylistForSongsDelete,
            PlaylistA,
            PlaylistB,
            PlaylistC,
            PlaylistZ
            );
        
        return dbx;
    }
}