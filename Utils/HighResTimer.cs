using System.Diagnostics;

namespace QuickSortComparison.Utils
{
    public static class HighResTimer
    {
        private static readonly double MillisecondsPerTick =
            1000.0 / Stopwatch.Frequency;

        public static double Measure(Action action)
        {
            var sw = Stopwatch.StartNew();
            action();
            sw.Stop();

            return sw.ElapsedTicks * MillisecondsPerTick;
        }
    }
}
