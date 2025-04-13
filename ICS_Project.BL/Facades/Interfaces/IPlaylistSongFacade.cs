using ICS_Project.BL.Models;
using ICS_Project.DAL.Interfaces;

namespace ICS_Project.BL.Facades.Interfaces;

public interface IPlaylistSongFacade
{
    Task SaveAsync(PlaylistSongDetailModel model, Guid playlistId);
    Task SaveAsync(PlaylistSongListModel model, Guid playlistId);
    Task DeleteAsync(Guid id);
}