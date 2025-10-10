using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES.Core.Common;

public class Entity : IEntity, ICreateAt, IUpdateAt
{
    /// <summary>
    /// 用户主键
    /// </summary>
    [Key] [Column(Order = 0)] [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; } = 0;

    /// <summary>
    /// 用户唯一键
    /// </summary>
    [Required] [Column(Order = 1)]
    public string Guid { get; set; } = string.Empty;

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateAt { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdateAt { get; set; }
}
