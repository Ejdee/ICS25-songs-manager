using ICS_Project.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS_Project.DAL;

public class IcsDbContext(DbContextOptions options): DbContext(options) 
{
    public DbSet<SongEntity> Songs { get; set; }
    public DbSet<PlaylistEntity> Playlists { get; set; }
}