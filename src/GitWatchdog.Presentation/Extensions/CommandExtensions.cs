using System.Windows.Input;
using GitWatchdog.Presentation.Command;

namespace GitWatchdog.Presentation.Extensions
{
    public static class CommandExtensions
    {
        public static void ExecuteCommandIfPossible(this ICommand command, object param = null)
        {
            if (command?.CanExecute(param)??false)
            {
                command.Execute(param);
            }
        }

        public static void RaiseCanExecuteChangedIfPossible(this ICommand command)
        {
            (command as ICanRaiseCanExecute)?.RaiseCanExecuteChanged();
        }
    }
}
