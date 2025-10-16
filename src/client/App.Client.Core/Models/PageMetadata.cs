using System.Windows.Shapes;
using App.Client.Core.Interfaces;

namespace App.Client.Core.Models;

public class PageMetadata : IPageMetadata
{
    public string Label { get; set; } = "Page Label";
    public Path Path { get; set; } = new Path();
    
    public required Type TargetType { get; set; }
}