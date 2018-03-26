using System;
using System.Diagnostics;
using AppKit;
using GitWatchdog.Presentation.Services;

namespace Gitwatchdog.MacOS.Services
{
    public class PlatformProvider : IPlatformProvider
    {
        public ProcessStartInfo GetTerminal()
        {
            return new ProcessStartInfo("/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal")
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                ErrorDialog = false,
                UseShellExecute = false
            };
        }

        public string BrowseFolder()
        {
            var panel = new NSOpenPanel
            {
                ReleasedWhenClosed = true,
                CanChooseDirectories = true,
                CanChooseFiles = false
            };

            var result = panel.RunModal();

            if(result != 1)
            {
                return null;
            }

            var directoryPath = panel.DirectoryUrl.RelativeString;

            return directoryPath;
        }
    }
}
