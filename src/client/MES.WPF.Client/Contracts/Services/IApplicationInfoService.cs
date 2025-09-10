using System;

namespace MES.WPF.Client.Contracts.Services
{
    public interface IApplicationInfoService
    {
        Version GetVersion();
    }
}
