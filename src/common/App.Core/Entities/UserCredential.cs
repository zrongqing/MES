using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Common;

namespace App.Core.Entities;

/// <summary>
/// 用户凭据表
/// </summary>
public class UserCredential : Entity
{
    public required string PasswordHash { get; set; }
 
    public long UserProfileId { get; set; }
    
    public string? Email { get; set; }
    
    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTime LastLoginAt { get; set; }
    
    /// <summary>
    /// 用户信息表
    /// </summary>
    [ForeignKey(nameof(UserProfileId))]
    public UserProfile UserProfile { get; set; } = null!; // EFCore的推荐做法，不启用懒加载，避免循环调用
    

    //写法在EFCore中抛弃，
    //public virtual UserProfile UserProfile { get; set; } = null!;
}