using System;
using System.IO;
using System.Reactive.Concurrency;

namespace GitWatchdog.Presentation
{
    public static class Constants
    {
#if DEBUG
        public const string ApplicationName = "GitWatchdog Dev";
#else

        public const string ApplicationName = "GitWatchdog";
#endif
        public static readonly string CurrentUserAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        
        public static readonly string AppDataFolder = Path.Combine(CurrentUserAppData, ApplicationName);
    }
}
