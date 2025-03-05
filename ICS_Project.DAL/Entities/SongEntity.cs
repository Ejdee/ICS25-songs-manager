using ICS_Project.DAL.Interfaces;

namespace ICS_Project.DAL;

public record SongEntity: IEntity
{
    public Guid Id { get; init; }
    public string Name { get; private set; }
    public string Description { get; init; }
    public string Genre { get; private set; }
    public float Size { get; private set; } //TODO: Is size necessary? Will we be using whole files or links??
    public int DurationInSeconds { get; private set; }
    public ArtistEntity ArtistEntity { get; private set; }
    public AlbumEntity AlbumEntity { get; private set; }
    public string SongUrl { get; private set; }

    public SongEntity(){}
    public SongEntity(string name, string description, string genre, int durationInSeconds, ArtistEntity artistEntity, AlbumEntity albumEntity, string songUrl)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        Genre = genre ?? throw new ArgumentNullException(nameof(genre));
        DurationInSeconds = durationInSeconds;
        ArtistEntity = artistEntity ?? throw new ArgumentNullException(nameof(artistEntity));
        AlbumEntity = albumEntity ?? throw new ArgumentNullException(nameof(albumEntity));
        SongUrl = songUrl ?? throw new ArgumentNullException(nameof(songUrl));
        artistEntity.AddSong(this);
        albumEntity.AddSong(this);
    }
    
}