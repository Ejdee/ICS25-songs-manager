using ICS_Project.BL.Models;
using ICS_Project.DAL.Entities;

namespace ICS_Project.BL.Mappers;

public class SongModelMapper
    : ModelMapperBase<SongEntity, SongListModel, SongDetailModel>
{
    public override SongListModel MapToListModel(SongEntity? entity)
        => entity is null
            ? SongListModel.Empty
            : new SongListModel
            {
                Id = entity.Id,
                Name = entity.Name,
                DurationInSeconds = entity.DurationInSeconds
            };

    public override SongDetailModel MapToDetailModel(SongEntity? entity)
        => entity is null
            ? SongDetailModel.Empty
            : new SongDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                DurationInSeconds = entity.DurationInSeconds,
                Genre = entity.Genre,
                Artist = entity.Artist,
                SongUrl = entity.SongUrl
            };

    public override SongEntity MapToEntity(SongDetailModel model)
        => new()
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            DurationInSeconds = model.DurationInSeconds,
            Genre = model.Genre,
            Artist = model.Artist,
            SongUrl = model.SongUrl
        };
}