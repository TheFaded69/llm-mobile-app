using Users.Contracts.V1.Requests;
using Users.Infrastructure.Repositories;

namespace Users.Application.Handlers;

public class DeleteUserHandler
{
    private readonly IUserRepository _userRepository;

    public DeleteUserHandler(IUserRepository  userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        await _userRepository.DeleteUserAsync(request.Id, cancellationToken);
    }
}