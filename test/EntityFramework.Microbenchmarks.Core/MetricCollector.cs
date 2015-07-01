using System;
using System.Diagnostics;

namespace EntityFramework.Microbenchmarks.Core
{
    public class MetricCollector
    {
        private bool _collecting;
        private readonly Scope _scope;
        private Stopwatch _timer = new Stopwatch();
        private long _cumulativeMemoryDelta;
        private long _memoryOnStartCollection;

        public MetricCollector()
        {
            _scope = new Scope(this);
        }

        public IDisposable StartCollection()
        {
            _collecting = true;
            _timer.Start();
            _memoryOnStartCollection = GetMemory();
            return _scope;
        }

        public void StopCollection()
        {
            if (_collecting)
            {
                _timer.Stop();
                _collecting = false;
                var currentMemory = GetMemory();
                _cumulativeMemoryDelta += currentMemory - _memoryOnStartCollection;
            }
        }

        public void Reset()
        {
            _timer.Reset();
            _cumulativeMemoryDelta = 0;
        }

        public long TimeEllapsed
        {
            get { return _timer.ElapsedMilliseconds; }
        }

        public long MemoryDelta
        {
            get { return _cumulativeMemoryDelta; }
        }

        private static long GetMemory()
        {
            for (var i = 0; i < 5; i++)
            {
                GC.GetTotalMemory(forceFullCollection: true);
            }

            return GC.GetTotalMemory(forceFullCollection: true);
        }

        private class Scope : IDisposable
        {
            private readonly MetricCollector _collector;


            public Scope(MetricCollector collector)
            {
                _collector = collector;
            }


            public void Dispose()
            {
                _collector.StopCollection();
            }
        }

    }
}
