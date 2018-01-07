using System.Threading;
using log4net;

namespace toofz
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StoreActivity : ProgressActivityBase<long>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="name"></param>
        public StoreActivity(ILog log, string name) : this(log, name, null) { }

        internal StoreActivity(ILog log, string name, IStopwatch stopwatch) : base("Store", log, name, stopwatch) { }

        private long rowsAffected;

        /// <summary>
        /// 
        /// </summary>
        public long RowsAffected => rowsAffected;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public override void Report(long value)
        {
            Interlocked.Add(ref rowsAffected, value);
        }

        #region IDisposable Implementation

        private bool disposed;

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            if (disposed) { return; }

            if (Log.IsInfoEnabled)
            {
                var rows = RowsAffected;
                var total = Stopwatch.Elapsed.TotalSeconds;
                var rate = (rows / total).ToString("F0");

                Log.Info($"{Category} {Name} complete -- {rows} rows affected over {total.ToString("F1")} seconds ({rate} rows per second).");
            }

            disposed = true;

            base.Dispose();
        }

        #endregion
    }
}
