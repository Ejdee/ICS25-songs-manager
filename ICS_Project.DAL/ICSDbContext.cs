using ICS_Project.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS_Project.DAL;

public class IcsDbContext(DbContextOptions options): DbContext(options)
{
    public DbSet<PlaylistSongEntity> PlaylistSongs => Set<PlaylistSongEntity>();
    public DbSet<SongEntity> Songs => Set<SongEntity>();
    public DbSet<PlaylistEntity> Playlists => Set<PlaylistEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PlaylistEntity>()
            .HasMany(s => s.PlaylistSongs)
            .WithOne(p => p.Playlist)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<SongEntity>()
            .HasMany<PlaylistSongEntity>()
            .WithOne(s => s.Song)
            .OnDelete(DeleteBehavior.Restrict);

    }
}