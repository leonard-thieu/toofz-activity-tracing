using System;
using log4net;

namespace toofz
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ActivityBase : IDisposable
    {
        internal ActivityBase(string category, ILog log, string name, IStopwatch stopwatch)
        {
            Category = category;
            Log = log ?? throw new ArgumentNullException(nameof(log));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Stopwatch = stopwatch ?? StopwatchAdapter.StartNew();

            if (Log.IsDebugEnabled) { Log.Debug($"Start {Category} {Name}"); }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string Category { get; }
        /// <summary>
        /// 
        /// </summary>
        protected ILog Log { get; }
        /// <summary>
        /// 
        /// </summary>
        protected string Name { get; }

        internal IStopwatch Stopwatch { get; }

        #region IDisposable Members

        private bool disposed;

        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {
            if (disposed) { return; }

            if (Log.IsDebugEnabled) { Log.Debug($"End {Category} {Name}"); }

            disposed = true;
        }

        #endregion
    }
}
