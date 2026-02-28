using Users.Contracts.V1.Requests;
using Users.Domain.Models;
using Users.Infrastructure.Repositories;

namespace Users.Application.Handlers;

public class AddUserHandler
{
    private readonly IUserRepository _userRepository;

    public AddUserHandler(IUserRepository  userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(AddUserRequest request, CancellationToken cancellationToken)
    {
        var user = new User()
        {
            UserName = request.UserName,
            Email = request.Email,
            UserType = request.UserType,
            Role = request.Role
        };

        await _userRepository.AddUserAsync(user, cancellationToken);
    }
}