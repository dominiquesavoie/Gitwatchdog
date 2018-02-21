using System;
using System.Windows.Input;

namespace GitWatchdog.Command
{
    public class Command : ICommand, ICanRaiseCanExecute
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        private bool _isRunning;

        public Command(Action<object> execute, Func<object,bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return (_canExecute?.Invoke(parameter) ?? true) && !_isRunning;
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                _isRunning = true;

                CanExecuteChanged?.Invoke(this, EventArgs.Empty);

                try
                {
                    _execute(parameter);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    _isRunning = false;

                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
                
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged;
    }
}
