// This file has been autogenerated from a class added in the UI designer.

using System;
using AppKit;
using GitWatchdog.Presentation.ViewModel;
using GitWatchdog.Presentation.Helpers;
using System.Reactive.Concurrency;
using Gitwatchdog.MacOS.TableViewSource;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace Gitwatchdog.MacOS
{
	public partial class GitwatchdogContainer : NSViewController
	{
        private MainViewModel ViewModel { set; get; }

        private GitwatchdogTableViewSource _tableViewSource;

        private SerialDisposable _addButtonSubscription = new SerialDisposable();

		public GitwatchdogContainer (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Hopefully, the CurrentThread scheduler is the MacOS dispatcher.
            DispatcherHelper.DefaultDispatcherScheduler = Scheduler.CurrentThread;
            ViewModel = new MainViewModel();
        }

		public override void ViewDidAppear()
		{
			base.ViewDidAppear();

            _tableViewSource = new GitwatchdogTableViewSource(ViewModel.Items, GitWatchdogList);

            _addButtonSubscription.Disposable = Observable.FromEventPattern<EventHandler, EventArgs>(
                    h => btnAdd.Activated += h,
                    h => btnAdd.Activated -= h,
                    DispatcherHelper.DefaultDispatcherScheduler)
                .Subscribe(_ => ViewModel.AddNewRepo.Execute(txtUrl.StringValue));
		}

		public override void ViewDidDisappear()
		{
			base.ViewDidDisappear();

            _addButtonSubscription.Disposable = null;
            _tableViewSource = null;
            GitWatchdogList.DataSource = null;
		}
	}
}
