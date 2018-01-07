using System.Diagnostics;
using Xunit;

namespace toofz.Tests
{
    public class StopwatchAdapterTests
    {
        public class StartNew
        {
            [DisplayFact(nameof(StopwatchAdapter))]
            public void ReturnsStartedStopwatchAdapter()
            {
                // Arrange -> Act
                var adapter = StopwatchAdapter.StartNew();

                // Assert
                Assert.IsAssignableFrom<StopwatchAdapter>(adapter);
                Assert.True(adapter.IsRunning);
            }
        }

        public class IsRunning
        {
            [DisplayFact(nameof(Stopwatch.IsRunning), nameof(Stopwatch))]
            public void ReturnsIsRunningFromStopwatch()
            {
                // Arrange
                var stopwatch = Stopwatch.StartNew();
                var adapter = new StopwatchAdapter(stopwatch);

                // Act -> Assert
                Assert.Equal(stopwatch.IsRunning, adapter.IsRunning);
            }
        }

        public class Elapsed
        {
            [DisplayFact(nameof(Stopwatch.Elapsed), nameof(Stopwatch))]
            public void ReturnsElapsedFromStopwatch()
            {
                // Arrange
                var stopwatch = Stopwatch.StartNew();
                var adapter = new StopwatchAdapter(stopwatch);
                adapter.Stop();

                // Act -> Assert
                Assert.Equal(stopwatch.Elapsed, adapter.Elapsed);
            }
        }

        public class ElapsedMilliseconds
        {
            [DisplayFact(nameof(Stopwatch.ElapsedMilliseconds), nameof(Stopwatch))]
            public void ReturnsElapsedMillisecondsFromStopwatch()
            {
                // Arrange
                var stopwatch = Stopwatch.StartNew();
                var adapter = new StopwatchAdapter(stopwatch);
                adapter.Stop();

                // Act -> Assert
                Assert.Equal(stopwatch.ElapsedMilliseconds, adapter.ElapsedMilliseconds);
            }
        }

        public class ElapsedTicks
        {
            [DisplayFact(nameof(Stopwatch.ElapsedTicks), nameof(Stopwatch))]
            public void ReturnsElapsedTicksFromStopwatch()
            {
                // Arrange
                var stopwatch = Stopwatch.StartNew();
                var adapter = new StopwatchAdapter(stopwatch);
                adapter.Stop();

                // Act -> Assert
                Assert.Equal(stopwatch.ElapsedTicks, adapter.ElapsedTicks);
            }
        }

        public class Reset
        {
            [DisplayFact]
            public void StopsMeasuringAndResetsElapsedTimeToZero()
            {
                // Arrange
                var adapter = StopwatchAdapter.StartNew();

                // Act
                adapter.Reset();

                // Assert
                Assert.False(adapter.IsRunning);
                Assert.Equal(0, adapter.ElapsedTicks);
            }
        }

        public class Restart
        {
            [DisplayFact]
            public void ResetsElapsedTimeAndStartsMeasuring()
            {
                // Arrange
                var adapter = StopwatchAdapter.StartNew();

                // Act
                adapter.Restart();

                // Assert
                Assert.True(adapter.IsRunning);
            }
        }

        public class Start
        {
            [DisplayFact]
            public void StartsMeasuring()
            {
                // Arrange
                var stopwatch = new Stopwatch();
                var adapter = new StopwatchAdapter(stopwatch);

                // Act
                adapter.Start();

                // Assert
                Assert.True(adapter.IsRunning);
            }
        }

        public class Stop
        {
            [DisplayFact]
            public void StopsMeasuring()
            {
                // Arrange
                var adapter = StopwatchAdapter.StartNew();

                // Act
                adapter.Stop();

                // Assert
                Assert.False(adapter.IsRunning);
            }
        }
    }
}
