using ICS_Project.DAL.Interfaces;

namespace ICS_Project.DAL;

public record PlaylistEntity
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    private readonly List<SongEntity> _songs = new List<SongEntity>();
    public IReadOnlyList<SongEntity> Songs => _songs;

    public PlaylistEntity() { }
    public PlaylistEntity(string name)
    {
        Name = name;
    }
    public void AddSongItem(SongEntity songEntity)
    {
        _songs.Add(songEntity);
    }

    public void RemoveSongItem(SongEntity songEntity)
    {
        _songs.Remove(songEntity);
    }

    // public List<SongEntity> GetAllSongs() => _songs;

    public List<SongEntity> GetSongsByArtist(string artistName) =>
        _songs.Where(s => s.ArtistEntity.Name == artistName).ToList();

    public List<SongEntity> GetSongsByAlbum(string albumTitle) =>
        _songs.Where(s => s.AlbumEntity.Name == albumTitle).ToList();
}