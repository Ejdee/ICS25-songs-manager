namespace Class_navrh.DAL;

public class PlaylistEntity : IEntity
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required string? Description { get; set; }

    public int NumberOfSongs { get; set; }


    //VRATIT A A POCHOPIT
    // Kolekcia skladieb, ktoré sú priradené k tomuto playlistu
    public ICollection<PlaylistMusicFileEntity> MusicFileEntities { get; init; } = new List<PlaylistMusicFileEntity>();
    //METODY:
    // removesong()
    // getTotalDuration().. zadanie
    // addsong()
    // NumberOfSongs()

}