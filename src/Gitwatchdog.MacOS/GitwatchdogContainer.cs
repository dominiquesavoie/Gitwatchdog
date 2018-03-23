// This file has been autogenerated from a class added in the UI designer.

using System;
using AppKit;
using GitWatchdog.Presentation.ViewModel;
using GitWatchdog.Presentation.Helpers;
using System.Reactive.Concurrency;
using Gitwatchdog.MacOS.TableViewSource;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using Gitwatchdog.MacOS.Services;
using GitWatchdog.Presentation.Extensions;

namespace Gitwatchdog.MacOS
{
	public partial class GitwatchdogContainer : NSViewController
	{
        private MainViewModel ViewModel { set; get; }

        private GitwatchdogTableViewSource _tableViewSource;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

		public GitwatchdogContainer (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Hopefully, the CurrentThread scheduler is the MacOS dispatcher thread.
            DispatcherHelper.DefaultDispatcherScheduler = Scheduler.CurrentThread;
            ViewModel = new MainViewModel()
            {
                PlatformProvider = new PlatformProvider()
            };

        }

		public override void ViewDidAppear()
		{
			base.ViewDidAppear();

            _tableViewSource = new GitwatchdogTableViewSource(ViewModel.Items, GitWatchdogList);
            _tableViewSource.DeleteCommand = ViewModel.DeleteRepo;

            Observable.FromEventPattern<EventHandler, EventArgs>(
                    h => btnAdd.Activated += h,
                    h => btnAdd.Activated -= h,
                    DispatcherHelper.DefaultDispatcherScheduler)
                .Subscribe(_ => ViewModel.AddNewRepo.ExecuteCommandIfPossible(txtUrl.StringValue))
                .DisposeWith(_subscriptions);

            Observable.FromEventPattern<EventHandler, EventArgs>(
                    h => btnRefresh.Activated += h,
                    h => btnRefresh.Activated -= h,
                    DispatcherHelper.DefaultDispatcherScheduler)
                .Subscribe(_ => ViewModel.RefreshCommand.ExecuteCommandIfPossible())
                .DisposeWith(_subscriptions);
		}

		public override void ViewDidDisappear()
		{
			base.ViewDidDisappear();

            _subscriptions.Clear();
            _tableViewSource = null;
            GitWatchdogList.DataSource = null;
		}
	}
}
