// This file has been autogenerated from a class added in the UI designer.

using System;
using AppKit;
using GitWatchdog.Presentation.ViewModel;
using GitWatchdog.Presentation.Helpers;
using System.Reactive.Concurrency;
using Gitwatchdog.MacOS.TableViewSource;
using System.Reactive.Disposables;
using Gitwatchdog.MacOS.Services;
using GitWatchdog.Presentation.Extensions;
using Gitwatchdog.MacOS.Extensions;
using Foundation;

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
            ViewModel = ViewModelProvider.ProvideMainViewModel();

        }

		public override void ViewDidAppear()
		{
			base.ViewDidAppear();

            _tableViewSource = new GitwatchdogTableViewSource(ViewModel.Items, GitWatchdogList);
            _tableViewSource.DeleteCommand = ViewModel.DeleteRepo;


            btnAdd.RegisterCommand(ViewModel.AddNewRepo, () => txtUrl.StringValue)
                  .DisposeWith(_subscriptions);

            btnRefresh.RegisterCommand(ViewModel.RefreshCommand)
                      .DisposeWith(_subscriptions);

            btnBrowse.RegisterCommand(ViewModel.BrowseCommand)
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
