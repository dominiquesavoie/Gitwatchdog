// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Gitwatchdog.MacOS
{
    [Register ("GitwatchdogContainer")]
    partial class GitwatchdogContainer
    {
        [Outlet]
        AppKit.NSButton btnAdd { get; set; }

        [Outlet]
        AppKit.NSTableView GitWatchdogList { get; set; }

        [Outlet]
        AppKit.NSTextField txtUrl { get; set; }
        
        void ReleaseDesignerOutlets ()
        {
            if (btnAdd != null) {
                btnAdd.Dispose ();
                btnAdd = null;
            }

            if (GitWatchdogList != null) {
                GitWatchdogList.Dispose ();
                GitWatchdogList = null;
            }

            if (txtUrl != null) {
                txtUrl.Dispose ();
                txtUrl = null;
            }
        }
    }
}
