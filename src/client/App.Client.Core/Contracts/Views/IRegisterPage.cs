using App.Client.Core.Interfaces;

namespace App.Client.Core.Contracts.Views;

public interface IRegisterPage<TViewModel> where TViewModel : class
{
    public IPageMetadata PageMetadata { get; }
}

// public class RegisterPage<TViewModel> : Page, IRegisterPage<TViewModel> where TViewModel : class
// {
//     public IPageMetadata PageMetadata { get; }
// }

