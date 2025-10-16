namespace App.AppServices.Models;

/// <summary>
/// 用户会议
/// </summary>
class UserSession
{
    public static string Token { get; set; } = string.Empty;
    public static bool IsAuthenticated => !string.IsNullOrEmpty(Token);
}