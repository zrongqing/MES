namespace App.Infrastructure.Requests;

public class LoginRequest : BaseRequest
{
    public string UserName { get; set; } = string.Empty; 
    public string Password { get; set; } = string.Empty;
}