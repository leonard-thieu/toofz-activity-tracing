using System.Threading;
using Humanizer;
using log4net;

namespace toofz
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DownloadActivity : ProgressActivityBase<long>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="name"></param>
        public DownloadActivity(ILog log, string name) : this(log, name, null) { }

        internal DownloadActivity(ILog log, string name, IStopwatch stopwatch) : base("Download", log, name, stopwatch) { }

        private long totalBytes;

        /// <summary>
        /// 
        /// </summary>
        public long TotalBytes => totalBytes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public override void Report(long value)
        {
            Interlocked.Add(ref totalBytes, value);
        }

        #region IDisposable Members

        private bool disposed;

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            if (disposed) { return; }

            if (Log.IsInfoEnabled)
            {
                var byteSize = totalBytes.Bytes();
                var elapsed = Stopwatch.Elapsed;
                var byteRate = byteSize.Per(elapsed);

                var size = byteSize.Humanize("#.#");
                var time = elapsed.TotalSeconds.ToString("F1");
                var rate = byteRate.Humanize("#.#").Replace("/", "p");

                Log.Info($"{Category} {Name} complete -- {size} over {time} seconds ({rate}).");
            }

            disposed = true;

            base.Dispose();
        }

        #endregion
    }
}
