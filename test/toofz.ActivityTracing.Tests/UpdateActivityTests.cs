using Humanizer;
using log4net;
using Moq;
using Xunit;

namespace toofz.Tests
{
    public class UpdateActivityTests
    {
        public UpdateActivityTests()
        {
            mockLog.Setup(l => l.IsInfoEnabled).Returns(true);

            activity = new UpdateActivity(mockLog.Object, name, mockStopwatch.Object);
        }

        private readonly Mock<ILog> mockLog = new Mock<ILog>();
        private readonly string name = "daily leaderboards";
        private readonly Mock<IStopwatch> mockStopwatch = new Mock<IStopwatch>();
        private readonly UpdateActivity activity;

        public class Constructor
        {
            [DisplayFact(nameof(UpdateActivity))]
            public void ReturnsUpdateActivity()
            {
                // Arrange
                var log = Mock.Of<ILog>();
                var name = "myName";

                // Act
                var activity = new UpdateActivity(log, name);

                // Assert
                Assert.IsAssignableFrom<UpdateActivity>(activity);
            }
        }

        public class DisposeMethod : UpdateActivityTests
        {
            [DisplayFact]
            public void LogsCompletionMessage()
            {
                // Arrange
                mockStopwatch.SetupGet(s => s.Elapsed).Returns((13.2).Seconds());

                // Act
                activity.Dispose();

                // Assert
                mockLog.Verify(l => l.Info("Update daily leaderboards complete after 13.2 s."));
            }

            [DisplayFact]
            public void DisposingMoreThanOnce_OnlyLogsCompletionMessageOnce()
            {
                // Arrange
                mockStopwatch.SetupGet(s => s.Elapsed).Returns((13.2).Seconds());

                // Act
                activity.Dispose();
                activity.Dispose();

                // Assert
                mockLog.Verify(l => l.Info("Update daily leaderboards complete after 13.2 s."), Times.Once);
            }
        }
    }
}
