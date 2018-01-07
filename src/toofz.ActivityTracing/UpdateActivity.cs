using System.Globalization;
using log4net;

namespace toofz
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UpdateActivity : ActivityBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="name"></param>
        public UpdateActivity(ILog log, string name) : this(log, name, null) { }

        internal UpdateActivity(ILog log, string name, IStopwatch stopwatch) : base("Update", log, name, stopwatch)
        {
            if (Log.IsInfoEnabled) { Log.Info($"{Category} {name} starting..."); }
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
                var duration = Stopwatch.Elapsed.TotalSeconds.ToString("F1", CultureInfo.CurrentCulture);
                Log.Info($"{Category} {Name} complete after {duration} s.");
            }

            disposed = true;

            base.Dispose();
        }

        #endregion
    }
}
