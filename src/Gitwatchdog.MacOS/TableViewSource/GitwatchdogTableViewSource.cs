using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Linq;
using AppKit;
using GitWatchdog.Presentation.Helpers;
using GitWatchdog.Presentation.Model;
using GitWatchdog.Presentation.Extensions;
using System.Linq;
using System.Collections.Generic;

namespace Gitwatchdog.MacOS.TableViewSource
{
    public class GitwatchdogTableViewSource : NSTableViewSource, IDisposable
    {
        private ObservableCollection<Item> _contents;

        private IDisposable _contentChangesSubscription;

        private NSTableView _tableView;

        private const string COLUMN_NAME = "COLUMN_NAME";
        private const string COLUMN_PATH = "COLUMN_PATH";
        private const string COLUMN_DELETE = "COLUMN_DELETE"
        private const string CellId = "GitWatchdogCell";

        public GitwatchdogTableViewSource(ObservableCollection<Item> list, NSTableView tableView)
        {
            _contents = list;

            _contentChangesSubscription = Observable
                .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                    h => _contents.CollectionChanged += h,
                    h => _contents.CollectionChanged -= h,
                    DispatcherHelper.DefaultDispatcherScheduler
                )
                .Select(args => args.EventArgs)
                .Do(args =>
                {
                    _tableView.ReloadData();
                })
                .CatchError()
                .Subscribe();

            _tableView = tableView;
            _tableView.Source = this;
        }

		public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
		{
            var isDeleteButton = tableColumn.Identifier.Equals(COLUMN_DELETE);

            if(isDeleteButton)
            {
                
            }

            var view = (NSTextField)tableView.MakeView(CellId, this);
            if (view == null)
            {
                view = new NSTextField();
                view.Identifier =  CellId;
                view.BackgroundColor = NSColor.Clear;
                view.Bordered = false;
                view.Selectable = false;
                view.Editable = false;
            }

            var data = _contents[(int)row];

            // Set up view based on the column and row
            switch (tableColumn.Identifier)
            {
                case COLUMN_NAME: 
                    view.StringValue = data.Name; 
                    break;
                case COLUMN_PATH:
                    view.StringValue = data.Path;
                    break;
            }

            return view;
		}

        public override nint GetRowCount(NSTableView tableView)
        {
            return _contents.Count;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _contentChangesSubscription?.Dispose();
            _tableView = null;
        }
    }
}
