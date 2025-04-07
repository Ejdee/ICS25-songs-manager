using ICS_Project.DAL.Entities;

namespace ICS_Project.DAL.Mappers;

public class SongEntityMapper : IEntityMapper<SongEntity>
{
    public void MapToExistingEntity(SongEntity existingEntity, SongEntity newEntity)
    {
        existingEntity.Name = newEntity.Name;
        existingEntity.Description = newEntity.Description;
        existingEntity.DurationInSeconds = newEntity.DurationInSeconds;
        existingEntity.Genre = newEntity.Genre;
        existingEntity.Artist = newEntity.Artist;
        existingEntity.SongUrl = newEntity.SongUrl;
    }
}