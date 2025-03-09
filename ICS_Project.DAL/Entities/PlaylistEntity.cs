namespace ICS_Project.DAL.Entities;

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
}