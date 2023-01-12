using System;
using System.Windows.Input;

namespace Project_1.Commands;

public class Command : ICommand
{
    #region Поля

    public event EventHandler? CanExecuteChanged;

    private readonly Action<object> _excute;

    private readonly Func<object, bool> _canExecute;

    #endregion

    #region Конструктор

    public Command(Action<object> excute, Func<object, bool> canExecute = null)
    {
        _excute = excute;

        _canExecute = canExecute;
    }

    #endregion

    #region Методы

    public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

    public void Execute(object? parameter) => _excute(parameter);

    #endregion
}