using Microsoft.EntityFrameworkCore;

namespace ICS_Project.DAL.UnitOfWork;

public class UnitOfWorkFactory : IUnitOfWorkFactory 
{
    private readonly IDbContextFactory<IcsDbContext> _dbContextFactory;

    public UnitOfWorkFactory(IDbContextFactory<IcsDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    
    public IUnitOfWork Create() => new UnitOfWork(_dbContextFactory.CreateDbContext());
}