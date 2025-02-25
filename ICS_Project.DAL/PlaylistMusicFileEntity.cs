namespace Class_navrh.DAL;

public record PlaylistMusicFileEntity : IEntity
{
    public Guid Id { get; set; }

    public required Guid MusicId { get; set; }

    public required Guid PlaylistId { get; set; }

    public required PlaylistEntity Playlist { get; init; }

    public required MusicFileEntity MusicFile { get; init; }

}