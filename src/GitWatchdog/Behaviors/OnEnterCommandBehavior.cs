using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GitWatchdog.Extensions;

namespace GitWatchdog.Behaviors
{
    public class OnEnterCommandBehavior : AutoDisposeBehavior<TextBox>
    {
        public static readonly DependencyProperty OnEnterCommandProperty = DependencyProperty.Register(
            "OnEnterCommand", typeof(ICommand), typeof(OnEnterCommandBehavior), new PropertyMetadata(default(ICommand)));

        public ICommand OnEnterCommand
        {
            get { return (ICommand) GetValue(OnEnterCommandProperty); }
            set { SetValue(OnEnterCommandProperty, value); }
        }

        protected override IDisposable OnAttach()
        {
            AssociatedObject.KeyUp += (sender, args) => { };
            return Observable
                .FromEventPattern<KeyEventHandler, KeyEventArgs>(
                    h => AssociatedObject.KeyUp += h,
                    h => AssociatedObject.KeyUp -= h,
                    DispatcherScheduler.Current
                )
                .Select(args => args.EventArgs)
                .Where(args => args.Key == Key.Enter)
                .Do(_ => OnEnterCommand.ExecuteCommandIfPossible(AssociatedObject.Text))
                .CatchError()
                .Subscribe();
        }
    }
}
