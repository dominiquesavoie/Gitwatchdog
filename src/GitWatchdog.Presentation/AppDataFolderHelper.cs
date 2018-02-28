using System;
using System.IO;
using System.Reactive.Concurrency;

namespace GitWatchdog.Presentation
{
    public static class AppDataFolderHelper
    {
        public static string ApplicationName;

        public static readonly string CurrentUserAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        
        public static readonly string AppDataFolder = Path.Combine(CurrentUserAppData, ApplicationName);
    }
}
