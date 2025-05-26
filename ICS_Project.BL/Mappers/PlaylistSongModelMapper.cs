using ICS_Project.BL.Models;
using ICS_Project.DAL.Entities;

namespace ICS_Project.BL.Mappers;

public class PlaylistSongModelMapper
    : ModelMapperBase<PlaylistSongEntity, PlaylistSongListModel, PlaylistSongDetailModel>
{
    public override PlaylistSongListModel MapToListModel(PlaylistSongEntity? entity)
        => entity?.Song is null
            ? PlaylistSongListModel.Empty
            : new PlaylistSongListModel
            {
                Id = entity.Id,
                SongId = entity.SongId,
                SongName = entity.Song.Name,
                SongDurationInSeconds = TimeSpan.FromSeconds(entity.Song.DurationInSeconds),
            };

    public override PlaylistSongDetailModel MapToDetailModel(PlaylistSongEntity? entity)
        => entity?.Song is null
            ? PlaylistSongDetailModel.Empty 
            : new PlaylistSongDetailModel
            {
                Id = entity.Id,
                SongId = entity.SongId,
                SongName = entity.Song.Name,
                SongDurationInSeconds = TimeSpan.FromSeconds(entity.Song.DurationInSeconds),
                Artist = entity.Song.Artist,
                Genre = entity.Song.Genre,
                SongUrl = entity.Song.SongUrl ?? string.Empty,
            };

    public PlaylistSongListModel MapToListModel(PlaylistSongDetailModel detailModel)
        => new()
        {
            Id = detailModel.Id,
            SongId = detailModel.SongId,
            SongName = detailModel.SongName,
            SongDurationInSeconds = detailModel.SongDurationInSeconds,
        };

    public void MapToExistingDetailModel(PlaylistSongDetailModel existingDetailModel, SongListModel song)
    {
        existingDetailModel.SongId = song.Id;
        existingDetailModel.SongName = song.Name;
        existingDetailModel.SongDurationInSeconds = song.DurationInSeconds;
    } 

    public override PlaylistSongEntity MapToEntity(PlaylistSongDetailModel model)
        => throw new NotImplementedException("This method is unsupported. Playlist ID is needed.");

    public PlaylistSongEntity MapToEntity(PlaylistSongDetailModel model, Guid playlistId)
        => new()
        {
            Id = model.Id,
            PlaylistId = playlistId,
            SongId = model.SongId,
            Song = null!,
            Playlist = null!
        };

    public PlaylistSongEntity MapToEntity(PlaylistSongListModel model, Guid playlistId)
        => new()
        {
            Id = model.Id,
            PlaylistId = playlistId,
            SongId = model.SongId,
            Song = null!,
            Playlist = null!
        };
}