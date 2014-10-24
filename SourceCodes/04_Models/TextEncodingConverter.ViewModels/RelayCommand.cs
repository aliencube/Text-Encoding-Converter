using System;
using System.Windows.Input;

namespace Aliencube.TextEncodingConverter.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Predicate<object> _onCanExecute;
        private readonly Action<object> _onExecute;

        public RelayCommand(Predicate<object> onCanExecute, Action<object> onExecute)
        {
            if (onCanExecute == null)
                throw new ArgumentNullException("onCanExecute");

            if (onExecute == null)
                throw new ArgumentNullException("onExecute");

            this._onCanExecute = onCanExecute;
            this._onExecute = onExecute;
        }

        public Predicate<object> OnCanExecute
        {
            get { return this._onCanExecute; }
        }

        public Action<object> OnExecute
        {
            get { return this._onExecute; }
        }

        public bool CanExecute(object parameter)
        {
            return this.OnCanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.OnExecute(parameter);
        }

        public event System.EventHandler CanExecuteChanged;
    }
}