using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OhMyWoodWorkerSimulator.Commands
{
    class delegateCommand : ICommand
    {
        private Action _action;

        public delegateCommand(Action action)
        {
            _action = action;
        }


        public void Execute(object parameter)
        {
            _action();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged;
    }
}
