using ICS_Project.DAL.Interfaces;

namespace ICS_Project.DAL.Repositories;

public interface IRepository<TEntity>
    where TEntity: class, IEntity
{
    IQueryable<TEntity> GetAll();
    ValueTask<bool> ExistsAsync(TEntity entity);
    TEntity InsertAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task DeleteAsync(Guid id);
}
