using App.Infrastructure.Dtos;
using App.Infrastructure.Requests;

namespace App.Server.Servers;

public interface IUserService
{
    Task<List<UserDto>?> GetAllAsync();

    Task<object?> LoginAsync(LoginRequest request);
}

public class UserService : IUserService
{
    public Task<List<UserDto>?> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<object?> LoginAsync(LoginRequest request)
    {
        throw new NotImplementedException();
    }
}