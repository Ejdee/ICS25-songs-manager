using ICS_Project.DAL.Interfaces;

namespace ICS_Project.DAL.Entities;

public record SongEntity: IEntity
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public string Description { get; init; }
    public string Genre { get; private set; }
    public int DurationInSeconds { get; private set; }
    public string Artist { get; init; }
    public string SongUrl { get; private set; }

    public SongEntity(string name, string description, string genre, int durationInSeconds, string artist, string songUrl)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        Genre = genre ?? throw new ArgumentNullException(nameof(genre));
        DurationInSeconds = durationInSeconds;
        Artist = artist;
        SongUrl = songUrl ?? throw new ArgumentNullException(nameof(songUrl));
    }
}