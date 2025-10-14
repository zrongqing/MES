using System.Windows.Controls;

namespace MES.Client.Core.Contracts.Services
{
    public interface IPageService
    {
        Type GetPageType(string key);

        Page GetPage(string key);
    }
}
