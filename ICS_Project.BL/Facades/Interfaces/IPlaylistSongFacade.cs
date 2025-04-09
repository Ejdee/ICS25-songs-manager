using ICS_Project.BL.Models;
using ICS_Project.DAL.Interfaces;

namespace ICS_Project.BL.Facades.Interfaces;

public interface IPlaylistSongFacade
{
    Task SaveAsync(PlaylistSongDetailModel model, Guid songId);
    Task SaveAsync(PlaylistSongListModel model, Guid songId);
    Task DeleteAsync(Guid id);
}