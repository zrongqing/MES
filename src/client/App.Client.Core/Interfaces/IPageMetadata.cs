using System.Windows.Shapes;

namespace App.Client.Core.Interfaces;

public interface IPageMetadata
{
    public string Label { get; set; } 
    public Path Path { get; set; } 
    public Type TargetType { get; set; }
}