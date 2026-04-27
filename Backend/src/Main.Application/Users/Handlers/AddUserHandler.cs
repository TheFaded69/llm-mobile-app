using Main.Contract.Users.V1.Requests;
using Main.Domain.Users.Models;
using Main.Infrastructure.Repositories;

namespace Main.Application.Users.Handlers;

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