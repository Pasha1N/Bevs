using System;
using System.Windows.Input;

namespace Bevs.Wpf.Commands
{
    public abstract class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public virtual bool CanExcute()
        {
            return false;
        }

        public abstract void Execute();

        bool ICommand.CanExecute(object parameter)
        {
            return CanExcute();
        }

        void ICommand.Execute(object parameter)
        {
            Execute();
        }

        public void OnCanExecuteChanged(EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }
    }
}