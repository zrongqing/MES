using MES.Core.Common;

namespace MES.Core.Entities;

/// <summary>
/// 用户信息表
/// </summary>
public class UserProfile : Entity
{
    /// <summary>
    /// 用户唯一名
    /// </summary>
    public required string UserName { get; set; }
    /// <summary>
    /// 用户显示的名字
    /// </summary>
    public string? DisplayName { get; set; }
    /// <summary>
    /// 用户邮箱
    /// </summary>
    public string? Email { get; set; }
    
    public UserCredential? Credential { get; set; }
}
