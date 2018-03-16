﻿using System;
using System.Diagnostics;
using GitWatchdog.Presentation.Services;

namespace Gitwatchdog.MacOS.Services
{
    public class PlatformProvider : IPlatformProvider
    {
        public ProcessStartInfo GetTerminal()
        {
            return new ProcessStartInfo("bash")
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