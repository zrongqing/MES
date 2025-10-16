using System.Security.Cryptography;

namespace App.Server.Security;

public class RefreshToken
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }

    public bool IsExpired => DateTime.UtcNow >= Expiration;
    
    public static RefreshToken Generate(int daysValid = 7)
    {
        return new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expiration = DateTime.UtcNow.AddDays(daysValid)
        };
    }
}