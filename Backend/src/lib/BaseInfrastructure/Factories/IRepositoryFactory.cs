using BaseDomain.Models;
using BaseInfrastructure.Repositories;

namespace BaseInfrastructure.Factories;

public interface IRepositoryFactory<TModelType, TKeyType> where TModelType : Entity<TKeyType>
{
    Task<Repository<TModelType, TKeyType>> CreateRepositoryAsync();
    
    Repository<TModelType, TKeyType> CreateRepository();
}