using BaseDomain.Models;
using BaseInfrastructure.Repositories;

namespace BaseInfrastructure.Factories;

public interface IRepositoryFactory<TModelType, TKeyType> where TModelType : Entity<TKeyType>
{
    Task<GenericRepository<TModelType, TKeyType>> CreateRepositoryAsync();
    
    GenericRepository<TModelType, TKeyType> CreateRepository();
}