using System;
using System.Windows.Input;

namespace Calculate.Wpf.Commands
{
    public sealed class DelegateCommand : ICommand
    {
        private readonly Action<object> _execute;

        public event EventHandler CanExecuteChanged = delegate { };

        public DelegateCommand(Action<object> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
