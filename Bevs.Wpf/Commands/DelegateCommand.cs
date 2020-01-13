using System;

namespace Bevs.Wpf.Commands
{
   public class DelegateCommand : Command
    {
        private Func<bool> canExecuteMethod;
        private Action executeMethod;

        public DelegateCommand(Action ExecuteMethod)
             : this(ExecuteMethod, () => true)
        {
        }

        public DelegateCommand(Action ExecuteMethod, Func<bool> CanExecuteMethod)
        {
            this.canExecuteMethod = CanExecuteMethod;
            this.executeMethod = ExecuteMethod;
        }


        public override void Execute()
        {
            executeMethod();
        }
        public override bool CanExcute()
        {
            return canExecuteMethod();
        }
    }
}