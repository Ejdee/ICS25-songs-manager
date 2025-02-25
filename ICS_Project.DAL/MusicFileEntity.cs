namespace Class_navrh.DAL
{
    public record MusicFileEntity : IEntity
    {
        public required Guid Id { get; set; }

        public required ArtistEntity ArtistEntity { get; set; }

        public required string Title { get; set; }

        public GenreEntity Genre {get; set;}

        public string Album { get; set; }

        public TimeSpan Duration { get; set; }

        // metody
        // play ()
        // delete()
        
    }
}
