using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SUP23_G4.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;

        public RelayCommand(Action<object> execute)
        {
            _execute = execute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}
