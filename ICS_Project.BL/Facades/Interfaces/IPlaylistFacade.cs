using ICS_Project.BL.Models;
using ICS_Project.BL.Models.Enums;
using ICS_Project.DAL.Entities;

namespace ICS_Project.BL.Facades.Interfaces;

public interface IPlaylistFacade : IFacade<PlaylistEntity, PlaylistListModel, PlaylistDetailModel>
{
    public Task<IEnumerable<PlaylistListModel>> SearchByNameAsync(string name);

    public Task<IEnumerable<PlaylistListModel>> GetSortedAsync(SortOptions sortOption,
        bool ascending = true);
}