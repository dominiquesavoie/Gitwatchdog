using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Text;

namespace GitWatchdog.Presentation.Helpers
{
    public static class DispatcherHelper
    {
        public static IScheduler DefaultDispatcherScheduler { get; set; }
    }
}
