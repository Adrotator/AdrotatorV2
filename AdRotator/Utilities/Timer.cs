using System;
using System.Threading;
using System.Threading.Tasks;

namespace AdRotator.Utilities
{
    public delegate void TimerCallback(object state);

    public sealed class Timer : CancellationTokenSource, IDisposable
    {
        public Timer(TimerCallback callback, object state, int dueTime, int period)
        {
            Task.Delay(dueTime, Token).ContinueWith(async (t, s) =>
            {
                await RunTimer(period, s);

            }, Tuple.Create(callback, state), CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
                TaskScheduler.Default);
        }

        public Timer(TimerCallback callback, object state, TimeSpan dueTime, TimeSpan period)
        {
            Task.Delay(dueTime, Token).ContinueWith(async (t, s) =>
            {
                await RunTimer(period, s);

            }, Tuple.Create(callback, state), CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
                TaskScheduler.Default);
        }

        private async Task RunTimer(int period, object s)
        {
            var tuple = (Tuple<TimerCallback, object>)s;

            while (true)
            {
                if (IsCancellationRequested)
                    break;
                Task.Run(() => tuple.Item1(tuple.Item2));
                await Task.Delay(period);
            }
        }

        private async Task RunTimer(TimeSpan period, object s)
        {
            var tuple = (Tuple<TimerCallback, object>)s;

            while (true)
            {
                if (IsCancellationRequested)
                    break;
                Task.Run(() => tuple.Item1(tuple.Item2));
                await Task.Delay(period);
            }
        }

        public new void Dispose() { base.Cancel(); }
    }
}
