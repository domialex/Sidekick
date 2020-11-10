using System;
using System.Windows.Input;

namespace Sidekick.Presentation.Wpf.Views.TrayIcon
{
    public class RelayCommand : ICommand
  {
    private readonly Action<object> execute;

    private readonly Predicate<object> canExecute;

    public RelayCommand(Action<object> execute)
       : this(execute, _ => true)
    {
    }

    public RelayCommand(Action<object> execute, Predicate<object> canExecute)
    {
      this.execute = execute ?? throw new ArgumentNullException("execute");
      this.canExecute = canExecute ?? throw new ArgumentNullException("canExecute");
    }

    public bool CanExecute(object parameter)
    {
      return canExecute != null && canExecute(parameter);
    }

    public void Execute(object parameter)
    {
      execute(parameter);
    }

    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }
  }
}
