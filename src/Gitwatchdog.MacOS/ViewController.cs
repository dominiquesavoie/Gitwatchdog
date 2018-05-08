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
