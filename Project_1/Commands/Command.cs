﻿using System;
using System.Windows.Input;

namespace Project_1.Commands;

public class Command : ICommand
{
    #region Поля

    public event EventHandler? CanExecuteChanged;

    private readonly Action<object> excute;

    private readonly Func<object, bool> canExecute;

    #endregion

    #region Конструктор

    public Command(Action<object> Excute, Func<object, bool> CanExecute = null)
    {
        excute = Excute;

        canExecute = CanExecute;
    }

    #endregion

    #region Методы

    public bool CanExecute(object? parameter) => canExecute?.Invoke(parameter) ?? true;

    public void Execute(object? parameter) => excute(parameter);

    #endregion
}