using App.Client.Core.Models;

namespace App.Client.Core.Contracts.Services;

public interface IApplicationInfoService
{
    Version GetVersion();
        
    List<PageMetadata>  GetNavigationPaneItems();
}