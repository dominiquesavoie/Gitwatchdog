using System.Reactive.Concurrency;
using System.Windows;
using System.Windows.Threading;
using GitWatchdog.Presentation.Helpers;

namespace GitWatchdog
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DispatcherHelper.DefaultDispatcherScheduler = new DispatcherScheduler(Dispatcher, DispatcherPriority.Normal);
        }
    }
}
