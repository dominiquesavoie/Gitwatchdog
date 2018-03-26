using System;
using System.Reactive.Linq;
using System.Windows.Input;
using AppKit;
using GitWatchdog.Presentation.Helpers;
using GitWatchdog.Presentation.Extensions;

namespace Gitwatchdog.MacOS.Extensions
{
    public static class NSButtonExtensions
    {
        public static IDisposable RegisterCommand(this NSButton nsButton, ICommand command, Func<object> commandParameterProvider = null)
        {
            return Observable.FromEventPattern<EventHandler, EventArgs>(
                    h => nsButton.Activated += h,
                    h => nsButton.Activated -= h,
                    DispatcherHelper.DefaultDispatcherScheduler)
                .Select(_ => commandParameterProvider?.Invoke())
                .Subscribe(command.ExecuteCommandIfPossible);
        }
    }
}
