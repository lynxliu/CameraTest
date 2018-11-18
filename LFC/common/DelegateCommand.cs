using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LFC.common
{
    public class DelegateCommand:ICommand
    {
        Action TargetAction { get; set; }
        Predicate<object> TargetPredicate { get; set; }
        public DelegateCommand(Action action)
        {
            TargetAction = action;
        }

        public bool CanExecute(object parameter)
        {
            if (TargetPredicate == null) return true;
            return TargetPredicate(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
                if (TargetAction != null)
                    TargetAction();
        }
    }

    public class DelegateCommand<T> : ICommand
    {
        Action<T> TargetAction { get; set; }
        Predicate<object> TargetPredicate { get; set; }
        public DelegateCommand(Action<T> action)
        {
            TargetAction = action;
        }
        public bool CanExecute(object parameter)
        {
            if (TargetPredicate == null) return true;
            return TargetPredicate(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
                if (TargetAction != null&&(parameter is T))
                    TargetAction((T)parameter);
        }
    }
}
