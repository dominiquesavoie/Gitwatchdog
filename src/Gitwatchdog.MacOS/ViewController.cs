using System;
using System.Reactive.Concurrency;
using AppKit;
using Foundation;
using GitWatchdog.Presentation.Helpers;
using GitWatchdog.Presentation.ViewModel;

namespace Gitwatchdog.MacOS
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        private MainViewModel ViewModel { get; } = new MainViewModel();

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Hopefully, the CurrentThread scheduler is the MacOS dispatcher.
            DispatcherHelper.DefaultDispatcherScheduler = Scheduler.CurrentThread;
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }
    }
}
