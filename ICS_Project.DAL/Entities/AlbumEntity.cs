using ICS_Project.DAL.Interfaces;

namespace ICS_Project.DAL;

public record AlbumEntity: IEntity
{
    public Guid Id { get; init; }
    public string Name { get; private set; }
    public string Description { get; init; }
    public int ReleaseYear { get; private set; }
    private readonly List<SongEntity> _songs = new List<SongEntity>();
    public IReadOnlyList<SongEntity> Songs => _songs;

    public AlbumEntity(){}
    public AlbumEntity(string name, string description, int releaseYear, ArtistEntity artistEntity)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        ReleaseYear = releaseYear;
        artistEntity.AddAlbum(this);
    }
    public void UpdateAlbum(string name, int releaseYear)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        ReleaseYear = releaseYear;
    }
    public void AddSong(SongEntity songEntity)
    {
        if (songEntity == null) throw new ArgumentNullException(nameof(songEntity));
        _songs.Add(songEntity);
    }
    public void RemoveSong(SongEntity songEntity)
    {
        if (songEntity == null) throw new ArgumentNullException(nameof(songEntity));
        _songs.Remove(songEntity);
    }
}