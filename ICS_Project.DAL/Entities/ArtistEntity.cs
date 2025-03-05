using ICS_Project.DAL.Interfaces;

namespace ICS_Project.DAL;

public record ArtistEntity: IEntity
{
    public Guid Id { get; init; }
    public string Name { get; private set; }
    public string Description { get; init; }
    public string Country { get; private set; }
    private readonly List<SongEntity> _songs = new List<SongEntity>();
    private readonly List<AlbumEntity> _albums = new List<AlbumEntity>();
    public IReadOnlyList<SongEntity> Songs => _songs;
    public IReadOnlyList<AlbumEntity> Albums => _albums;

    public ArtistEntity(string name, string description, string country)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Country = country ?? throw new ArgumentNullException(nameof(country));
    }
    public void UpdateArtist(string name, string country)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Country = country ?? throw new ArgumentNullException(nameof(country));
    }

    public void AddSong(SongEntity songEntity)
    {
        if (songEntity == null) throw new ArgumentNullException(nameof(songEntity));
        _songs.Add(songEntity);
    }

    public void AddAlbum(AlbumEntity albumEntity)
    {
        if (albumEntity == null) throw new ArgumentNullException(nameof(albumEntity));
        _albums.Add(albumEntity);
    }
}