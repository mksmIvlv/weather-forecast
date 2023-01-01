using System;
using System.Windows.Input;

namespace Module_19.Commands
{
    internal class Command : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private readonly Action<object> excute;

        private readonly Func<object, bool> canExecute;
        public Command(Action<object> Excute, Func<object, bool> CanExecute = null)
        {
            excute = Excute;

            canExecute = CanExecute;
        }
        public bool CanExecute(object? parameter) => canExecute?.Invoke(parameter) ?? true;

        public void Execute(object? parameter) => excute(parameter);
    }
}
