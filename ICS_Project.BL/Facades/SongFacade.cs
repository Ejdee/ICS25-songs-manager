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

        var query = repository.GetAll().Where(s => s.Name.Contains(name));

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
    public async Task<IEnumerable<SongListModel>> GetSortedAsync(SongSortOption sortOption, bool ascending = true)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<SongEntity> repository = uow.GetRepository<SongEntity, SongEntityMapper>();

        IQueryable<SongEntity> query = repository.GetAll();

        // Apply sorting
        switch (sortOption)
        {
            case SongSortOption.Name:
                query = ascending
                    ? query.OrderBy(s => s.Name)
                    : query.OrderByDescending(s => s.Name);
                break;

            case SongSortOption.Duration:
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
    public async Task<SongDetailModel> SaveAsync(SongDetailModel model)
    {
        return await base.SaveAsync(model);
    }
}
