using MES.Client.Core.Models;

namespace MES.Client.Core.Contracts.Services
{
    public interface IApplicationInfoService
    {
        Version GetVersion();
        
        List<PageMetadata>  GetNavigationPaneItems();
    }
}
