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
	[Register ("Logs")]
	partial class Logs
	{
		[Outlet]
		AppKit.NSTextView txtLogs { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (txtLogs != null) {
				txtLogs.Dispose ();
				txtLogs = null;
			}
		}
	}
}
