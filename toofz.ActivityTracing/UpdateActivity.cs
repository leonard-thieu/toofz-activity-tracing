﻿using System.Globalization;
using log4net;

namespace toofz
{
    public sealed class UpdateActivity : ActivityBase
    {
        public UpdateActivity(ILog log, string name) : this(log, name, null) { }

        internal UpdateActivity(ILog log, string name, IStopwatch stopwatch) : base("Update", log, name, stopwatch)
        {
            if (Log.IsInfoEnabled) { Log.Info($"{Category} {name} starting..."); }
        }

        #region IDisposable Members

        private bool disposed;

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
