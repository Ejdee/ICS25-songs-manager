using System.Collections;
using System.Reflection;
using ICS_Project.BL.Facades.Interfaces;
using ICS_Project.BL.Models;
using ICS_Project.BL.Mappers.Interfaces;
using ICS_Project.DAL.Entities;
using ICS_Project.DAL.Interfaces;
using ICS_Project.DAL.Mappers;
using ICS_Project.DAL.Repositories;
using ICS_Project.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace ICS_Project.BL.Facades;

public class FacadeBase<TEntity, TListModel, TDetailModel, TEntityMapper>(
        IUnitOfWorkFactory unitOfWorkFactory,
        IModelMapper<TEntity, TListModel, TDetailModel> modelMapper)
    : IFacade<TEntity, TListModel, TDetailModel>
    where TEntity : class, IEntity
    where TListModel : IModel
    where TDetailModel : class, IModel
    where TEntityMapper : IEntityMapper<TEntity>, new()
{
    protected readonly IModelMapper<TEntity, TListModel, TDetailModel> ModelMapper = modelMapper;
    protected readonly IUnitOfWorkFactory UnitOfWorkFactory = unitOfWorkFactory;

    protected virtual ICollection<string> IncludesNavigationPathDetail => new List<string>();

    public async Task DeleteAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        try
        {
            await uow.GetRepository<TEntity, TEntityMapper>().DeleteAsync(id).ConfigureAwait(false);
            await uow.CommitAsync().ConfigureAwait(false);
        }
        catch (DbUpdateException e)
        {
            throw new InvalidOperationException("Entity deletion failed.", e);
        }
    }

    public virtual async Task<TDetailModel?> GetAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IQueryable<TEntity> query = CreateQueryWithIncludes(uow);
        TEntity? entity = await query.SingleOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);

        return entity is null
            ? null
            : ModelMapper.MapToDetailModel(entity);
    }
    
    public virtual async Task<IEnumerable<TListModel>> GetAsync()
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IQueryable<TEntity> query = CreateQueryWithIncludes(uow);

        List<TEntity> entities = await query.ToListAsync().ConfigureAwait(false);
            
        return ModelMapper.MapToListModel(entities);
    }

    public virtual async Task<TDetailModel> SaveAsync(TDetailModel model)
    {
        TDetailModel result;
        GuardCollectionsAreNotSet(model);
        TEntity entity = ModelMapper.MapToEntity(model);
        
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<TEntity> repository = uow.GetRepository<TEntity, TEntityMapper>();

        if (await repository.ExistsAsync(entity).ConfigureAwait(false))
        {
            TEntity updatedEntity = await repository.UpdateAsync(entity).ConfigureAwait(false);
            result = ModelMapper.MapToDetailModel(updatedEntity);
        }
        else
        {
            entity.Id = Guid.NewGuid();
            TEntity insertedEntity = repository.Insert(entity);
            result = ModelMapper.MapToDetailModel(insertedEntity);
        }
        
        await uow.CommitAsync().ConfigureAwait(false);
        return result;
    }

    public static void GuardCollectionsAreNotSet(TDetailModel model)
    {
        IEnumerable<PropertyInfo> collectionProperties = model
            .GetType()
            .GetProperties()
            .Where(i => typeof(ICollection).IsAssignableFrom(i.PropertyType));

        foreach (PropertyInfo collectionProperty in collectionProperties)
        {
            if (collectionProperty.GetValue(model) is ICollection { Count: > 0 })
            {
                throw new InvalidOperationException(
                    "Current BL and DAL infrastructure disallows insert or update of models with adjacent collections.");
            }
        }
    }

    private IQueryable<TEntity> CreateQueryWithIncludes(IUnitOfWork uow)
    {
        IQueryable<TEntity> query = uow
            .GetRepository<TEntity, TEntityMapper>()
            .GetAll();

        foreach (string includePath in IncludesNavigationPathDetail)
        {
            query = query.Include(includePath);
        }
            
        return query;
    }
}