using BaseDomain.Models;
using BaseInfrastructure.DbContext;
using BaseInfrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BaseInfrastructure.Factories;

public class RepositoryFactory<TModelType, TKeyType>(IDbContextFactory<DataContext> contextFactory) : IRepositoryFactory<TModelType, TKeyType>
    where TModelType : Entity<TKeyType>
{
    public async Task<Repository<TModelType, TKeyType>> CreateRepositoryAsync() => new(await contextFactory.CreateDbContextAsync());
    
    public Repository<TModelType, TKeyType> CreateRepository() => new(contextFactory.CreateDbContext());
}