using Microsoft.EntityFrameworkCore;

namespace ICS_Project.DAL;

public class IcsDbContext(DbContextOptions options): DbContext(options) 
{
    public DbSet<SongEntity> Songs { get; set; }
    public DbSet<ArtistEntity> Artists { get; set; }
    public DbSet<AlbumEntity> Albums { get; set; }
    public DbSet<PlaylistEntity> Playlists { get; set; }
}