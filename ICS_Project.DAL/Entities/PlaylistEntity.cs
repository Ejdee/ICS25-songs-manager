﻿using ICS_Project.DAL.Interfaces;

namespace ICS_Project.DAL.Entities;

public record PlaylistEntity : IEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    
    public string? ImageUrl { get; set; }
    public ICollection<PlaylistSongEntity> PlaylistSongs { get; init; } = new List<PlaylistSongEntity>();
}