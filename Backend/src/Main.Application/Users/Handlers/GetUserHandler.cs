using Main.Contract.Users.V1.Requests;
using Main.Contract.Users.V1.Responses;
using Main.Infrastructure.Repositories;

namespace Main.Application.Users.Handlers;

public class GetUserHandler
{
    private readonly IUserRepository _userRepository;

    public GetUserHandler(IUserRepository  userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<GetUserResponse?> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        var user =  await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (user == null) 
            return null;
        
        return new GetUserResponse()
        {
            Id = user.Id,
            Email = user.Email,
            Role = user.Role,
            UserType = user.UserType,
            UserName = user.UserName,
        };
    }
}