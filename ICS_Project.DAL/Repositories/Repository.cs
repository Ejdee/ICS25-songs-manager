using ICS_Project.DAL.Interfaces;
using ICS_Project.DAL.Mappers;
using Microsoft.EntityFrameworkCore;

namespace ICS_Project.DAL.Repositories;

public class Repository<TEntity>(
    DbContext dbContext,
    IEntityMapper<TEntity> entityMapper)
    : IRepository<TEntity>
    where TEntity : class, IEntity
{
    private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public IQueryable<TEntity> GetAll() => _dbSet;

    public async ValueTask<bool> ExistsAsync(TEntity entity) => entity.Id != Guid.Empty
        && await _dbSet.AnyAsync(e => e.Id == entity.Id).ConfigureAwait(false);

    public TEntity InsertAsync(TEntity entity) => _dbSet.Add(entity).Entity;

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        TEntity existingEntity = await _dbSet.SingleAsync(e => e.Id == entity.Id).ConfigureAwait(false);
        entityMapper.MapToExistingEntity(existingEntity, entity);
        return existingEntity;
    }

    public async Task DeleteAsync(Guid id) => _dbSet.Remove(await _dbSet.SingleAsync(e => e.Id == id).ConfigureAwait(false));
}