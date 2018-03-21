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
using System.Windows.Input;

namespace Gitwatchdog.MacOS.TableViewSource
{
    public class GitwatchdogTableViewSource : NSTableViewSource, IDisposable
    {
        private ObservableCollection<Item> _contents;

        private IDisposable _contentChangesSubscription;

        private NSTableView _tableView;

        private const string COLUMN_NAME = "COLUMN_NAME";
        private const string COLUMN_PATH = "COLUMN_PATH";
        private const string COLUMN_DELETE = "COLUMN_DELETE";
        private const string CellId = "GitWatchdogCell";

        public ICommand DeleteCommand { get; set; }

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

        public void OnDelete(object sender, EventArgs args)
        {
            var button = (NSButton)sender;
            DeleteCommand.ExecuteCommandIfPossible(_contents[(int)button.Tag]);
        }

		public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
		{
            var isDeleteButton = tableColumn.Identifier.Equals(COLUMN_DELETE);

            var data = _contents[(int)row];
            if(isDeleteButton)
            {
                var buttonView = (NSButton)tableView.MakeView(CellId, this);
                if(buttonView == null)
                {
                    buttonView = new NSButton();
                    buttonView.Title = "Delete";
                    buttonView.Activated += OnDelete;
                }
                buttonView.Tag = row;

                return buttonView;
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
