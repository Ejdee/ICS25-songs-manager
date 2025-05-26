using ICS_Project.BL.Facades.Interfaces;
using ICS_Project.BL.Mappers;
using ICS_Project.BL.Models;
using ICS_Project.DAL.Entities;
using ICS_Project.DAL.Mappers;
using ICS_Project.DAL.Repositories;
using ICS_Project.DAL.UnitOfWork;

namespace ICS_Project.BL.Facades;

public class PlaylistSongFacade(IUnitOfWorkFactory unitOfWorkFactory, PlaylistSongModelMapper playlistSongModelMapper)
    :
        FacadeBase<PlaylistSongEntity, PlaylistSongListModel, PlaylistSongDetailModel, PlaylistSongEntityMapper>(unitOfWorkFactory, playlistSongModelMapper), IPlaylistSongFacade
{
    public async Task SaveAsync(PlaylistSongListModel model, Guid playlistId)
    {
        PlaylistSongEntity entity = playlistSongModelMapper.MapToEntity(model, playlistId);
        
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<PlaylistSongEntity> repository =
            uow.GetRepository<PlaylistSongEntity, PlaylistSongEntityMapper>();

        if (await repository.ExistsAsync(entity))
        {
            await repository.UpdateAsync(entity);
            await uow.CommitAsync();
        }
        else
        {
            repository.Insert(entity);
            await uow.CommitAsync();
        }
    }


    public async Task SaveAsync(PlaylistSongDetailModel model, Guid playlistId)
    {
        PlaylistSongEntity entity = playlistSongModelMapper.MapToEntity(model, playlistId);
        
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<PlaylistSongEntity> repository =
            uow.GetRepository<PlaylistSongEntity, PlaylistSongEntityMapper>();

        repository.Insert(entity);
        await uow.CommitAsync();
    }
}