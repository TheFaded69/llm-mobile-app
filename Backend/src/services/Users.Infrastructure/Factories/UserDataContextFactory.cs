using BaseInfrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Users.Infrastructure.DbContext;

namespace Users.Infrastructure.Factories;

public class UserDataContextFactory: IDbContextFactory<DataContext>
{
    private readonly DbContextOptions<UserDataContext> _options;

    public UserDataContextFactory(DbContextOptions<UserDataContext> options)
    {
        _options = options;
    }

    public DataContext CreateDbContext()
        => new UserDataContext(_options);
}