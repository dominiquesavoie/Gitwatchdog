using System;
using System.IO;

namespace GitWatchdog.Presentation
{
    public static class AppDataFolderHelper
    {
        private const string ApplicationName = "GitWatchdog";

        public static readonly string CurrentUserAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        
        public static string AppDataFolder => Path.Combine(CurrentUserAppData, ApplicationName);
    }
}
