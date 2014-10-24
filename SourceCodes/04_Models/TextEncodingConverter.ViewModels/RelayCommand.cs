﻿using System;
using System.Windows.Input;

namespace Aliencube.TextEncodingConverter.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Predicate<object> _onCanExecute;
        private readonly Action<object> _onExecute;

        public RelayCommand(Predicate<object> onCanExecute, Action<object> onExecute)
        {
            this._onCanExecute = onCanExecute;
            this._onExecute = onExecute;
        }

        public Predicate<object> OnCanExecute
        {
            get { return this._onCanExecute; }
        }

        public Action<object> OnExecuted
        {
            get { return this._onExecute; }
        }

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