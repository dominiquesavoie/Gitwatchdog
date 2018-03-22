using System;
using System.Diagnostics;

namespace GitWatchdog.Presentation.Services
{
    public interface IPlatformProvider
    {
        ProcessStartInfo GetTerminal();

        string BrowseFolder();
    }
}
