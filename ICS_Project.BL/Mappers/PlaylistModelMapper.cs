using ICS_Project.BL.Models;
using ICS_Project.DAL.Entities;

namespace ICS_Project.BL.Mappers;

public class PlaylistModelMapper(PlaylistSongModelMapper playlistSongModelMapper)
    : ModelMapperBase<PlaylistEntity, PlaylistListModel, PlaylistDetailModel>
{
    public override PlaylistListModel MapToListModel(PlaylistEntity? entity)
        => entity is null
            ? PlaylistListModel.Empty
            : new PlaylistListModel
            {
                Id = entity.Id,
                Name = entity.Name,
                DurationInSeconds = GetTotalDuration(entity),
                SongCount = GetSongCount(entity)
            };

    public override PlaylistDetailModel MapToDetailModel(PlaylistEntity? entity)
        => entity is null
            ? PlaylistDetailModel.Empty
            : new PlaylistDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                DurationInSeconds = GetTotalDuration(entity),
                SongCount = GetSongCount(entity),
                Songs = entity.PlaylistSongs
                    .Select(playlistSongModelMapper.MapToListModel)
                    .ToObservableCollection()
            };

    public override PlaylistEntity MapToEntity(PlaylistDetailModel model)
        => new()
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
        };
    
    private static TimeSpan GetTotalDuration(PlaylistEntity playlistEntity)
        => TimeSpan.FromSeconds(playlistEntity.PlaylistSongs?.Sum(x => x.Song.DurationInSeconds.TotalSeconds) ?? 0);
    
    private static int GetSongCount(PlaylistEntity playlistEntity)
        => playlistEntity.PlaylistSongs?.Count ?? 0;
}