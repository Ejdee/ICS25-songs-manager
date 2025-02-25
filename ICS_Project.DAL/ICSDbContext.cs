using Microsoft.EntityFrameworkCore;

namespace ICS_Project.DAL;

public class IcsDbContext(DbContextOptions options): DbContext(options) 
{
    public DbSet<Song> Songs { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Album> Albums { get; set; }
}