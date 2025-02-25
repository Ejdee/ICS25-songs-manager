using ICS_Project.DAL.Entities;
using ICS_Project.DAL.Interfaces;

namespace ICS_Project.DAL;

public record Album: IEntity
{
    public Guid Id { get; init; }
    public string Name { get; private set; }
    public string Description { get; init; }
    public int ReleaseYear { get; private set; }
    private readonly List<Song> _songs = new List<Song>();
    public IReadOnlyList<Song> Songs => _songs;

    public Album(){}
    public Album(string name, string description, int releaseYear, Artist artist)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        ReleaseYear = releaseYear;
        artist.AddAlbum(this);
    }
    public void UpdateAlbum(string name, int releaseYear)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        ReleaseYear = releaseYear;
    }
    public void AddSong(Song song)
    {
        if (song == null) throw new ArgumentNullException(nameof(song));
        _songs.Add(song);
    }
    public void RemoveSong(Song song)
    {
        if (song == null) throw new ArgumentNullException(nameof(song));
        _songs.Remove(song);
    }
}