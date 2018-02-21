using System;
using System.IO;

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

        public static string AppDataFolder => Path.Combine(CurrentUserAppData, ApplicationName);
    }
}
