using System;
using System.Diagnostics;
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
            return null;
        }
    }
}
