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
                ImageUrl = entity.ImageUrl,
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
                ImageUrl = entity.ImageUrl,
                DurationInSeconds = GetTotalDuration(entity),
                SongCount = GetSongCount(entity),
                Songs = playlistSongModelMapper.MapToListModel(entity.PlaylistSongs)
                    .ToObservableCollection()
            };

    public override PlaylistEntity MapToEntity(PlaylistDetailModel model)
        => new()
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            ImageUrl = model.ImageUrl,
        };
    
    private static TimeSpan GetTotalDuration(PlaylistEntity playlistEntity)
        => TimeSpan.FromSeconds(playlistEntity.PlaylistSongs.Sum(x => x.Song.DurationInSeconds));
    
    private static int GetSongCount(PlaylistEntity playlistEntity)
        => playlistEntity.PlaylistSongs.Count;
}