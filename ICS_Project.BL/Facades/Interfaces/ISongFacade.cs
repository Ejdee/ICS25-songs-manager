using ICS_Project.BL.Models;
using ICS_Project.BL.Models.Enums;
using ICS_Project.DAL.Entities;

namespace ICS_Project.BL.Facades.Interfaces;

public interface ISongFacade : IFacade<SongEntity, SongListModel, SongDetailModel>
{
    public Task<IEnumerable<SongListModel>> SearchByNameAsync(string name);
    public Task<IEnumerable<SongListModel>> FilterByGenreAsync(string genre);
    
    public Task<IEnumerable<SongListModel>> GetSortedAsync(SortOptions sortOption, bool ascending = true);
}