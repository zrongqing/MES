using Microsoft.AspNetCore.Identity;

namespace App.Server.Models;

public class ApplicationUser : IdentityUser
{
    // 扩展字段（如需要）
    public string FullName { get; set; }
}