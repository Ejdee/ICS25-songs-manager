using ICS_Project.DAL.Entities;

namespace ICS_Project.DAL.Mappers;

public class PlaylistEntityMapper : IEntityMapper<PlaylistEntity>
{
    public void MapToExistingEntity(PlaylistEntity existingEntity, PlaylistEntity newEntity)
    {
        existingEntity.Name = newEntity.Name;
        existingEntity.Description = newEntity.Description;
        existingEntity.ImageUrl = newEntity.ImageUrl;
    }
}