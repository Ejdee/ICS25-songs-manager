namespace ICS_Project.DAL.Entities;

public record MusicCatalog
{
    private readonly List<Song> _songs = new List<Song>();

    public void AddSong(Song song)
    {
        if (song == null) throw new ArgumentNullException(nameof(song));
        _songs.Add(song);
    }

    public List<Song> GetSongsByArtist(string artistName) =>
        _songs.Where(s => s.Artist.Name == artistName).ToList();

    public List<Song> GetSongsByAlbum(string albumTitle) =>
        _songs.Where(s => s.Album.Name == albumTitle).ToList();
}