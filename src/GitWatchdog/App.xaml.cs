using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using GitWatchdog.Presentation;
using GitWatchdog.Presentation.Helpers;

namespace GitWatchdog
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

#if DEBUG
        public const string ApplicationName = "GitWatchdog Dev";
#else
        public const string ApplicationName = "GitWatchdog";
#endif
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AppDataFolderHelper.ApplicationName = ApplicationName;

            DispatcherHelper.DefaultDispatcherScheduler = new DispatcherScheduler(Dispatcher, DispatcherPriority.Normal);
        }
    }
}
