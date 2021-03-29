using System;
using System.Windows.Input;

namespace DummyChess.Mvvm
{
    public class DelegateCommand : ICommand
    {
        private readonly Action _action;

        public DelegateCommand(Action action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => _action?.Invoke();

        public event EventHandler CanExecuteChanged;
    }

    public class DelegateCommand<TPayload> : ICommand
    {
        private readonly Predicate<TPayload> _canExecute;
        private readonly Action<TPayload> _action;

        public DelegateCommand(Action<TPayload> action)
        {
            _action = action;
        }

        public DelegateCommand(Action<TPayload> action, Predicate<TPayload> canExecute)
         : this(action)
        {
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke((TPayload) parameter) ?? true;

        public void Execute(object parameter) => _action?.Invoke((TPayload)parameter);

        public event EventHandler CanExecuteChanged;
    }
}
