using System;
using System.Windows.Controls;

namespace MES.WPF.Client.Contracts.Services
{
    public interface IPageService
    {
        Type GetPageType(string key);

        Page GetPage(string key);
    }
}
