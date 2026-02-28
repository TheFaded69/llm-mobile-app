using BaseInfrastructure.Factories;
using Microsoft.EntityFrameworkCore;
using Users.Domain.Models;

namespace Users.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IRepositoryFactory<User, Guid> _repositoryFactory;

    public UserRepository(IRepositoryFactory<User, Guid> repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }
    
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();
        
        return await repository.GetAsync(id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();
        
        return await repository
            .Query
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task AddUserAsync(User user, CancellationToken cancellationToken)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();
        
        repository.Insert(user);

        await repository.CommitAsync();
    }

    public async Task UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();
                
        repository.Update(user);
        
        await repository.CommitAsync();
    }

    public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();
        
        var existUser = await repository.GetAsync(id, cancellationToken) ?? throw new Exception($"Такого пользователя [{id}] не существует");
        
        repository.Delete(existUser);
        
        await repository.CommitAsync();
    }
}