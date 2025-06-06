using ICS_Project.DAL.Repositories;
using ICS_Project.DAL.Interfaces;
using ICS_Project.DAL.Mappers;
using Microsoft.EntityFrameworkCore;

namespace ICS_Project.DAL.UnitOfWork;

public sealed class UnitOfWork(DbContext dbContext) : IUnitOfWork
{
    private readonly DbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    
    public IRepository<TEntity> GetRepository<TEntity, TEntityMapper>()
        where TEntity : class, IEntity 
        where TEntityMapper : IEntityMapper<TEntity>, new()
        => new Repository<TEntity>(_dbContext, new TEntityMapper());
    
    public async Task CommitAsync() => await _dbContext.SaveChangesAsync().ConfigureAwait(false);
    public async ValueTask DisposeAsync() => await _dbContext.DisposeAsync().ConfigureAwait(false);
}