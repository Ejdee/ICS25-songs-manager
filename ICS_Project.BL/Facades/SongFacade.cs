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

public class SongFacade(IUnitOfWorkFactory unitOfWorkFactory, SongModelMapper modelMapper) 
    : 
        FacadeBase<SongEntity, SongListModel, SongDetailModel, SongEntityMapper>(unitOfWorkFactory, modelMapper), ISongFacade
{
    // Returns a list of songs that contain the provided name
    public async Task<IEnumerable<SongListModel>> SearchByNameAsync(string name)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<SongEntity> repository = uow.GetRepository<SongEntity, SongEntityMapper>();

        var query = repository.GetAll().Where(s => s.Name.ToLower().Contains(name.ToLower()));

        var entities = await query.ToListAsync();
        return entities.Select(e => ModelMapper.MapToListModel(e));
    }

    // Returns a filtered list of songs based on the provided genre
    public async Task<IEnumerable<SongListModel>> FilterByGenreAsync(string genre)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<SongEntity> repository = uow.GetRepository<SongEntity, SongEntityMapper>();

        var query = repository.GetAll().Where(s => s.Genre.Equals(genre));

        var entities = await query.ToListAsync();
        return entities.Select(e => ModelMapper.MapToListModel(e));
    }
    
    // Returns a sorted list of songs based on the provided sort option
    public async Task<IEnumerable<SongListModel>> GetSortedAsync(SortOptions sortOption, bool ascending = true)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<SongEntity> repository = uow.GetRepository<SongEntity, SongEntityMapper>();

        IQueryable<SongEntity> query = repository.GetAll();

        // Apply sorting
        switch (sortOption)
        {
            case SortOptions.SongName:
                query = ascending
                    ? query.OrderBy(s => s.Name)
                    : query.OrderByDescending(s => s.Name);
                break;

            case SortOptions.SongDuration:
                query = ascending
                    ? query.OrderBy(s => s.DurationInSeconds)
                    : query.OrderByDescending(s => s.DurationInSeconds);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(sortOption), sortOption, null);
        }

        var entities = await query.ToListAsync();
        return entities.Select(e => ModelMapper.MapToListModel(e));
    }
    public async Task<IEnumerable<SongListModel>> GetAllAsync()
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<SongEntity> repository = uow.GetRepository<SongEntity, SongEntityMapper>();

        var entities = await repository.GetAll().ToListAsync();
        return entities.Select(e => ModelMapper.MapToListModel(e));
    }
    public override async Task<SongDetailModel> SaveAsync(SongDetailModel model)
    {
        return await base.SaveAsync(model);
    }
    
    public new async Task DeleteAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        var playlistSongRepo = uow.GetRepository<PlaylistSongEntity, PlaylistSongEntityMapper>();
        
        var links = await playlistSongRepo
            .GetAll()
            .Where(ps => ps.SongId == id)
            .ToListAsync();

        foreach (var link in links)
        {
            await playlistSongRepo.DeleteAsync(link.Id);
        }

        var songRepo = uow.GetRepository<SongEntity, SongEntityMapper>();
        await songRepo.DeleteAsync(id);

        await uow.CommitAsync();
    }
}
