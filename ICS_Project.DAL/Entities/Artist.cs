using ICS_Project.DAL.Interfaces;

namespace ICS_Project.DAL;

public record Artist: IEntity
{
    public Guid Id { get; init; }
    public string Name { get; private set; }
    public string Description { get; init; }
    public string Country { get; private set; }
    private readonly List<Song> _songs = new List<Song>();
    private readonly List<Album> _albums = new List<Album>();
    public IReadOnlyList<Song> Songs => _songs;
    public IReadOnlyList<Album> Albums => _albums;

    public Artist(string name, string description, string country)
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

    public void AddSong(Song song)
    {
        if (song == null) throw new ArgumentNullException(nameof(song));
        _songs.Add(song);
    }

    public void AddAlbum(Album album)
    {
        if (album == null) throw new ArgumentNullException(nameof(album));
        _albums.Add(album);
    }
}