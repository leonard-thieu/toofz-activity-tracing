using Humanizer;
using log4net;
using Moq;
using Xunit;

namespace toofz.Tests
{
    public class StoreActivityTests
    {
        public StoreActivityTests()
        {
            mockLog.Setup(l => l.IsInfoEnabled).Returns(true);

            activity = new StoreActivity(mockLog.Object, name, mockStopwatch.Object);
        }

        private readonly Mock<ILog> mockLog = new Mock<ILog>();
        private readonly string name = "entries";
        private readonly Mock<IStopwatch> mockStopwatch = new Mock<IStopwatch>();
        private readonly StoreActivity activity;

        public class Constructor
        {
            [DisplayFact(nameof(StoreActivity))]
            public void ReturnsStoreActivity()
            {
                // Arrange
                var log = Mock.Of<ILog>();
                var name = "myName";

                // Act
                var activity = new StoreActivity(log, name);

                // Assert
                Assert.IsAssignableFrom<StoreActivity>(activity);
            }
        }

        public class RowsAffectedProperty : StoreActivityTests
        {
            [DisplayFact]
            public void ReturnsRowsAffected()
            {
                // Arrange
                activity.Report(20);

                // Act
                var rowsAffected = activity.RowsAffected;

                // Assert
                Assert.Equal(20, rowsAffected);
            }
        }

        public class ReportMethod : StoreActivityTests
        {
            [DisplayFact]
            public void AddsValueToRowsAffected()
            {
                // Arrange -> Act
                activity.Report(1);
                activity.Report(1);

                // Assert
                Assert.Equal(2, activity.RowsAffected);
            }
        }

        public class DisposeMethod : StoreActivityTests
        {
            [DisplayFact]
            public void LogsCompletionMessage()
            {
                // Arrange
                activity.Report(759225);
                mockStopwatch.SetupGet(s => s.Elapsed).Returns((6.42).Seconds());

                // Act
                activity.Dispose();

                // Assert
                mockLog.Verify(l => l.Info("Store entries complete -- 759225 rows affected over 6.4 seconds (118259 rows per second)."));
            }

            [DisplayFact]
            public void DisposingMoreThanOnce_OnlyLogsCompletionMessageOnce()
            {
                // Arrange
                activity.Report(759225);
                mockStopwatch.SetupGet(s => s.Elapsed).Returns((6.42).Seconds());

                // Act
                activity.Dispose();
                activity.Dispose();

                // Assert
                mockLog.Verify(l => l.Info("Store entries complete -- 759225 rows affected over 6.4 seconds (118259 rows per second)."), Times.Once);
            }
        }
    }
}
