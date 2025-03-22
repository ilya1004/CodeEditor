using System.Windows.Input;

namespace CodeEditor.Core.Commands;

public class RelayCommand(Action execute, Func<bool>? canExecute = null) : ICommand
{
    public bool CanExecute(object? parameter)
    {
        return canExecute == null || canExecute();
    }

    public void Execute(object? parameter)
    {
        execute();
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}