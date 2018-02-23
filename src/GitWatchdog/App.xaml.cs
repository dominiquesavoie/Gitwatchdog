using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
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
