using BaseInfrastructure.DbContext;
using Main.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.Factories;

public class MainDataContextFactory: IDbContextFactory<DataContext>
{
    private readonly DbContextOptions<MainDataContext> _options;

    public MainDataContextFactory(DbContextOptions<MainDataContext> options)
    {
        _options = options;
    }

    public DataContext CreateDbContext()
        => new MainDataContext(_options);
}