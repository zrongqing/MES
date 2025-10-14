using System.Windows.Controls;
using MES.Client.Core.Interfaces;

namespace MES.Client.Core.Contracts.Views;

public interface IRegisterPage<TViewModel> where TViewModel : class
{
    public IPageMetadata PageMetadata { get; }
}

// public class RegisterPage<TViewModel> : Page, IRegisterPage<TViewModel> where TViewModel : class
// {
//     public IPageMetadata PageMetadata { get; }
// }

