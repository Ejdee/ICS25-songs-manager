using ICS_Project.DAL.Entities;

namespace ICS_Project.DAL.Mappers;

public class PlaylistSongEntityMapper : IEntityMapper<PlaylistSongEntity>
{
    public void MapToExistingEntity(PlaylistSongEntity existingEntity, PlaylistSongEntity newEntity)
    {
        existingEntity.PlaylistId = newEntity.PlaylistId; 
        existingEntity.SongId = newEntity.SongId;
    }
}