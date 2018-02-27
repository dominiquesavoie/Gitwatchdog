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


        public GitwatchdogTableViewSource(ObservableCollection<Item> list)
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
                    switch(args.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            HandleAdd(args.NewItems.OfType<Item>().ToArray());
                            break;
                    }
                })
                .CatchError()
                .Subscribe();
        }

        private void HandleAdd(IList<Item> itemsToAdd)
        {
            
        }

        public override NSCell GetCell(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            return base.GetCell(tableView, tableColumn, row);
        }

        public override nint GetRowCount(NSTableView tableView)
        {
            return base.GetRowCount(tableView);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _contentChangesSubscription?.Dispose();
        }
    }
}
