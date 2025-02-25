namespace Class_navrh.DAL;

public class ArtistEntity : IEntity
{
    public Guid Id { get; set; }

    public required string Name { get; set; }


    // metody:
    // getsongsbyartist()
}