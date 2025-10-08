using System;
using System.Windows.Input;
using MES.WPF.Client.Helpers;

namespace MES.WPF.Client.ViewModels
{
    public class ShellDialogViewModel : Observable
    {
        private ICommand _closeCommand;

        public ICommand CloseCommand => _closeCommand ?? (_closeCommand = new RelayCommand(OnClose));

        public Action<bool?> SetResult { get; set; }

        public ShellDialogViewModel()
        {
        }

        private void OnClose()
        {
            bool result = true;
            SetResult(result);
        }
    }
}
