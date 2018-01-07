using Humanizer;
using log4net;
using Moq;
using Xunit;

namespace toofz.Tests
{
    public class DownloadActivityTests
    {
        public DownloadActivityTests()
        {
            mockLog.Setup(l => l.IsInfoEnabled).Returns(true);

            activity = new DownloadActivity(mockLog.Object, name, mockStopwatch.Object);
        }

        private readonly Mock<ILog> mockLog = new Mock<ILog>();
        private readonly string name = "leaderboards";
        private readonly Mock<IStopwatch> mockStopwatch = new Mock<IStopwatch>();
        private readonly DownloadActivity activity;

        public class Constructor
        {
            [DisplayFact(nameof(DownloadActivity))]
            public void ReturnsDownloadActivity()
            {
                // Arrange
                var log = Mock.Of<ILog>();
                var name = "leaderboards";

                // Act
                var activity = new DownloadActivity(log, name);

                // Assert
                Assert.IsAssignableFrom<DownloadActivity>(activity);
            }
        }

        public class TotalBytesProperty : DownloadActivityTests
        {
            [DisplayFact]
            public void ReturnsTotalBytes()
            {
                // Arrange
                activity.Report(21);
                activity.Report(21);

                // Act
                var totalBytes = activity.TotalBytes;

                // Assert
                Assert.Equal(42, totalBytes);
            }
        }

        public class ReportMethod : DownloadActivityTests
        {
            [DisplayFact]
            public void AddsValueToTotalBytes()
            {
                // Arrange -> Act
                activity.Report(1);
                activity.Report(1);

                // Assert
                Assert.Equal(2, activity.TotalBytes);
            }
        }

        public class DisposeMethod : DownloadActivityTests
        {
            [DisplayFact]
            public void LogsSizeTimeAndRate()
            {
                // Arrange
                activity.Report((long)(26.3).Megabytes().Bytes);
                mockStopwatch.SetupGet(s => s.Elapsed).Returns((10.34).Seconds());

                // Act
                activity.Dispose();

                // Assert
                mockLog.Verify(l => l.Info("Download leaderboards complete -- 26.3 MB over 10.3 seconds (2.5 MBps)."));
            }

            [DisplayFact]
            public void DisposingMoreThanOnce_LogsSizeTimeAndRateOnlyOnce()
            {
                // Arrange
                activity.Report((long)(26.3).Megabytes().Bytes);
                mockStopwatch.SetupGet(s => s.Elapsed).Returns((10.34).Seconds());

                // Act
                activity.Dispose();
                activity.Dispose();

                // Assert
                mockLog.Verify(l => l.Info("Download leaderboards complete -- 26.3 MB over 10.3 seconds (2.5 MBps)."), Times.Once);
            }
        }
    }
}
