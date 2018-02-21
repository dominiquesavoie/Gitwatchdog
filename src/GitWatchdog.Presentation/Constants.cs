using System;
using System.IO;
using System.Reactive.Concurrency;

namespace GitWatchdog
{
    public static class Constants
    {
#if DEBUG
        public static readonly string ApplicationName = "GitWatchdog Dev";
#else

        public static readonly string ApplicationName = "GitWatchdog";
#endif
        public static string CurrentUserAppData => 
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public static IScheduler DispatcherScheduler = Scheduler.CurrentThread;

        public static string AppDataFolder => Path.Combine(CurrentUserAppData, ApplicationName);
    }
}
