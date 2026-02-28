using BaseInfrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Users.Infrastructure.DbContext;

namespace Users.Infrastructure.Adapters;

public class DataContextFactoryAdapter : IDbContextFactory<DataContext>
{
    private readonly IDbContextFactory<UserDataContext> _inner;

    public DataContextFactoryAdapter(IDbContextFactory<UserDataContext> inner)
    {
        _inner = inner;
    }

    public DataContext CreateDbContext() => _inner.CreateDbContext();
}