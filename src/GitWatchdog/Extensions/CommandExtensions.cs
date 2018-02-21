using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using GitWatchdog.Command;

namespace GitWatchdog.Extensions
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
