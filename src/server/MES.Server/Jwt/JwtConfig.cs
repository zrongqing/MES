namespace MES.Server.Jwt;

public class JwtConfig
{
    /// <summary>
    /// 密钥
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;
    
    /// <summary>
    /// 发布者
    /// </summary>
    public string Issuer { get; set; } = string.Empty;
    
    /// <summary>
    /// 接受者
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// 过期时间
    /// </summary>
    public int Expired { get; set; } = 60;
}
