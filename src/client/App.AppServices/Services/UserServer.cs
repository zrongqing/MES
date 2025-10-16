using App.AppServices.DTOs;
using App.Infrastructure.Responses;

namespace App.AppServices.Services;

public class UserServer
{
    private readonly ApiClient _apiClient;
    
    UserServer(ApiClient apiApiClient)
    {
        _apiClient = apiApiClient;
    }

    public async Task LoginAsync(string username, string password)
    {
        var request = new LoginRequest
        {
            Username = username,
            Password = password
        };

        // _client.PostAsync<string>()

        var response = await _apiClient.PostAsync<LoginResponse>("api/users", request);
        
        
        return;
    }
}