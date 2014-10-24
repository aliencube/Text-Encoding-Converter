using System.Windows.Input;

namespace Aliencube.TextEncodingConverter.ViewModels
{
    public class RelayCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            throw new System.NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new System.NotImplementedException();
        }

        public event System.EventHandler CanExecuteChanged;
    }
}