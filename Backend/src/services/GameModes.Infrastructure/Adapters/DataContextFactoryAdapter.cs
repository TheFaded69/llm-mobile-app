using BaseInfrastructure.DbContext;
using GameModes.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace GameModes.Infrastructure.Adapters;

public class DataContextFactoryAdapter : IDbContextFactory<DataContext>
{
    private readonly IDbContextFactory<GameModesDataContext> _inner;

    public DataContextFactoryAdapter(IDbContextFactory<GameModesDataContext> inner)
    {
        _inner = inner;
    }

    public DataContext CreateDbContext() => _inner.CreateDbContext();
}
