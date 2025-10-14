using System.Windows.Shapes;
using MES.Client.Core.Interfaces;

namespace MES.Client.Core.Models;

public class PageMetadata : IPageMetadata
{
    public string Label { get; set; } = "Page Label";
    public Path Path { get; set; } = new Path();
    
    public required Type TargetType { get; set; }
}
