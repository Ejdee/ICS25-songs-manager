using ICS_Project.DAL.Entities;
using ICS_Project.DAL.Interfaces;

namespace ICS_Project.DAL;

public record Song: IEntity
{
    public Guid Id { get; init; }
    public string Name { get; private set; }
    public string Description { get; init; }
    public string Genre { get; private set; }
    public float Size { get; private set; } //TODO: Is size necessary? Will we be using whole files or links??
    public int DurationInSeconds { get; private set; }
    public Artist Artist { get; private set; }
    public Album Album { get; private set; }
    public string SongUrl { get; private set; }

    public Song(){}
    public Song(string name, string description, string genre, int durationInSeconds, Artist artist, Album album, string songUrl)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        Genre = genre ?? throw new ArgumentNullException(nameof(genre));
        DurationInSeconds = durationInSeconds;
        Artist = artist ?? throw new ArgumentNullException(nameof(artist));
        Album = album ?? throw new ArgumentNullException(nameof(album));
        SongUrl = songUrl ?? throw new ArgumentNullException(nameof(songUrl));
        artist.AddSong(this);
        album.AddSong(this);
    }
    
}