using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using Plugin.Connectivity;

namespace GitWatchdog.Presentation.Extensions
{
    public static class ObservableExtensions
    {
        public static void OnNext(this ISubject<Unit> source)
        {
            source.OnNext(Unit.Default);
        }

        public static IObservable<Unit> SelectUnit<T>(this IObservable<T> source)
        {
            return source.Select(_ => Unit.Default);
        }


        public static IObservable<Unit> WhereIsConnected<T>(this IObservable<T> source)
        {
            return source
                .Where(_ => CrossConnectivity.Current.IsConnected)
                .SelectMany(_ => CrossConnectivity.Current.IsRemoteReachable("http://www.google.com"))
                .Where(isConnected => isConnected)
                .SelectUnit();
        }

        public static IObservable<T> CatchError<T>(this IObservable<T> source, [CallerMemberName]string callerName = "Unknown", [CallerLineNumber] int lineNumber = -1)
        {
            return source
                .Materialize()
                .Select(n =>
                {
                    if (n.Kind != NotificationKind.OnError)
                    {
                        return n;
                    }
                    if (Debugger.IsAttached)
                    {
                        var error = n.Exception;
                        Console.WriteLine(error.Message);
                        Console.WriteLine(error.StackTrace);
                    }

                    return Notification.CreateOnNext(default(T));
                })
                .Dematerialize();
        }
    }
}
