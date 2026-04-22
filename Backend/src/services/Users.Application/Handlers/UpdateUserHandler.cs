using Users.Contracts.V1.Requests;
using Users.Infrastructure.Repositories;

namespace Users.Application.Handlers;

public class UpdateUserHandler
{
    private readonly IUserRepository _userRepository;

    public UpdateUserHandler(IUserRepository  userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken)
                   ?? throw new Exception($"Такого пользователя [{request.Id}] не существует");
        
        user.UserType = request.UserType;
        user.UserName = request.UserName;
        user.Email = request.Email;
        user.Role = request.Role;

        await _userRepository.UpdateUserAsync(user, cancellationToken);
    }
}