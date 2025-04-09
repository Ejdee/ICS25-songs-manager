using ICS_Project.BL.Facades.Interfaces;
using ICS_Project.BL.Mappers;
using ICS_Project.BL.Models;
using ICS_Project.DAL.Entities;
using ICS_Project.DAL.Mappers;
using ICS_Project.DAL.UnitOfWork;

namespace ICS_Project.BL.Facades;

public class PlaylistFacade(IUnitOfWorkFactory unitOfWorkFactory, PlaylistModelMapper modelMapper)
    :
        FacadeBase<PlaylistEntity, PlaylistListModel, PlaylistDetailModel, PlaylistEntityMapper>(unitOfWorkFactory, modelMapper), IPlaylistFacade
{
    protected override ICollection<string> IncludesNavigationPathDetail => 
        new [] {$"{nameof(PlaylistEntity.PlaylistSongs)}.{nameof(PlaylistSongEntity.Song)}"};
}