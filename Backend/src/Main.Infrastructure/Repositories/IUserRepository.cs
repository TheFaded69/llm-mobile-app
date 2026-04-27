using Main.Domain.Users.Models;

namespace Main.Infrastructure.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    
    Task AddUserAsync(User user, CancellationToken cancellationToken);
    
    Task UpdateUserAsync(User user, CancellationToken cancellationToken);
    
    Task DeleteUserAsync(Guid id, CancellationToken cancellationToken);
}