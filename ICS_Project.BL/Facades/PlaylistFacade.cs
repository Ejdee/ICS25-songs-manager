using ICS_Project.BL.Facades.Interfaces;
using ICS_Project.BL.Mappers;
using ICS_Project.BL.Models;
using ICS_Project.BL.Models.Enums;
using ICS_Project.DAL.Entities;
using ICS_Project.DAL.Mappers;
using ICS_Project.DAL.Repositories;
using ICS_Project.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;


namespace ICS_Project.BL.Facades;

public class PlaylistFacade(IUnitOfWorkFactory unitOfWorkFactory, PlaylistModelMapper modelMapper)
    :
        FacadeBase<PlaylistEntity, PlaylistListModel, PlaylistDetailModel, PlaylistEntityMapper>(unitOfWorkFactory, modelMapper), IPlaylistFacade
{
    protected override ICollection<string> IncludesNavigationPathDetail => 
        new [] {$"{nameof(PlaylistEntity.PlaylistSongs)}.{nameof(PlaylistSongEntity.Song)}"};
    
    public async Task<IEnumerable<PlaylistListModel>> SearchByNameAsync(string name)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<PlaylistEntity> repository = uow.GetRepository<PlaylistEntity, PlaylistEntityMapper>();
        // added include to getting correct data
        var query = repository 
            .GetAll()
            .Include(p => p.PlaylistSongs)
            .ThenInclude(ps => ps.Song)
            .Where(p => p.Name.Contains(name));
        var entities = await query.ToListAsync();
        return entities.Select(e => ModelMapper.MapToListModel(e));
    }

    public async Task<IEnumerable<PlaylistListModel>> GetSortedAsync(SortOptions sortOption, bool ascending = true)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<PlaylistEntity> repository = uow.GetRepository<PlaylistEntity, PlaylistEntityMapper>();

        // added include to getting correct data
        IQueryable<PlaylistEntity> query = repository.GetAll()
            .Include(p => p.PlaylistSongs)
            .ThenInclude(ps => ps.Song);

        // Apply sorting 
        switch (sortOption)
        {
            case SortOptions.PlaylistName:
                query = ascending
                    ? query.OrderBy(p => p.Name)
                    : query.OrderByDescending(p => p.Name);
                break;

            case SortOptions.PlaylistSongCount:
                query = ascending
                    ? query.OrderBy(p => p.PlaylistSongs.Count)
                    : query.OrderByDescending(p => p.PlaylistSongs.Count);
                break;

            case SortOptions.PlaylistDuration:
                query = ascending
                    ? query.OrderBy(p => p.PlaylistSongs.Sum(ps => ps.Song.DurationInSeconds))
                    : query.OrderByDescending(p => p.PlaylistSongs.Sum(ps => ps.Song.DurationInSeconds));
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(sortOption), sortOption, null);
        }

        var entities = await query.ToListAsync();
        return entities.Select(e => ModelMapper.MapToListModel(e));
    }
}