using System.Diagnostics;
using GitWatchdog.Presentation.Services;

namespace GitWatchdog.Services
{
    public class PlatformProvider : IPlatformProvider
    {
        public ProcessStartInfo GetTerminal()
        {
            return new ProcessStartInfo("cmd")
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                ErrorDialog = false,
                UseShellExecute = false
            };
        }
    }
}
