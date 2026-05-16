using Main.Contract.Users.V1.Requests;
using Main.Infrastructure.Repositories;

namespace Main.Application.Users.Handlers;

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