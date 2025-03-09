using Microsoft.EntityFrameworkCore.Design;

namespace ICS_Project.DAL.Factories;

/// <summary>
///     EF Core CLI migration generation uses this DbContext to create model and migration
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<IcsDbContext>
{
    private readonly DbContextSqLiteFactory _dbContextSqLiteFactory = new("playlist-manager.db");

    public IcsDbContext CreateDbContext(string[] args) => _dbContextSqLiteFactory.CreateDbContext();
}
