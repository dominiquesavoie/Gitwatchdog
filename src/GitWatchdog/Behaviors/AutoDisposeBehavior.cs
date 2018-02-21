using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Interactivity;

namespace GitWatchdog.Behaviors
{
    /// <summary>
    /// This abstract class is used to facilitate management of Rx Subscriptions in behaviors.
    /// </summary>
    /// <typeparam name="T">A DependencyObject uppon wich this behavior is attached.</typeparam>
    public abstract class AutoDisposeBehavior<T> : Behavior<T> where T: DependencyObject
    {
        protected abstract IDisposable OnAttach();
        private readonly SerialDisposable _subsription = new SerialDisposable();

        protected void ReAttach()
        {
            _subsription.Disposable = null;
            _subsription.Disposable = OnAttach();
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            _subsription.Disposable = OnAttach();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            _subsription.Disposable = null;
        }
    }
}
