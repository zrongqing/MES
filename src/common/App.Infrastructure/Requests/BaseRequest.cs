namespace App.Infrastructure.Requests;

public class BaseRequest
{
    /// <summary>当前用户Id（可从Token或Header自动注入）</summary>
    public string? UserId { get; set; }

    /// <summary>请求追踪ID（日志用）</summary>
    public string? TraceId { get; set; }

    /// <summary>请求时间戳</summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}