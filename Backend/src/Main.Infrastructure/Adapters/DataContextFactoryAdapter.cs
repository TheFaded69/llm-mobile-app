using BaseInfrastructure.DbContext;
using Main.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.Adapters;

public class DataContextFactoryAdapter : IDbContextFactory<DataContext>
{
    private readonly IDbContextFactory<MainDataContext> _inner;

    public DataContextFactoryAdapter(IDbContextFactory<MainDataContext> inner)
    {
        _inner = inner;
    }

    public DataContext CreateDbContext() => _inner.CreateDbContext();
}