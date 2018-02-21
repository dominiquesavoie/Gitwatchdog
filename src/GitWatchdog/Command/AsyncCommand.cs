using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GitWatchdog.Command
{
    public class AsyncCommand : ICommand, ICanRaiseCanExecute
    {
        private readonly Func<object, Task> _execute;
        private readonly Func<object, bool> _canExecute;

        private bool _isRunning;

        public AsyncCommand(Func<object, Task> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return (_canExecute?.Invoke(parameter) ?? true) && !_isRunning;
        }

        public async void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                _isRunning = true;

                CanExecuteChanged?.Invoke(this, EventArgs.Empty);

                try
                {
                    await _execute(parameter);
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

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
