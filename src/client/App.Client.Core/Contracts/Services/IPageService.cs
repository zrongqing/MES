using System.Windows.Controls;

namespace App.Client.Core.Contracts.Services;

public interface IPageService
{
    Type GetPageType(string key);

    Page GetPage(string key);
}