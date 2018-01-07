using System;
using log4net;
using Moq;
using Xunit;

namespace toofz.Tests
{
    public class ActivityBaseTests
    {
        public ActivityBaseTests()
        {
            mockLog.Setup(l => l.IsDebugEnabled).Returns(true);

            activity = new ActivityBaseAdapter(category, mockLog.Object, name, mockStopwatch.Object);
        }

        private readonly string category = "myCategory";
        private readonly Mock<ILog> mockLog = new Mock<ILog>();
        private readonly string name = "myName";
        private readonly Mock<IStopwatch> mockStopwatch = new Mock<IStopwatch>();
        private readonly ActivityBaseAdapter activity;

        public class Constructor
        {
            [Fact]
            public void LogIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var category = "myCategory";
                ILog log = null;
                var name = "myName";
                var stopwatch = Mock.Of<IStopwatch>();

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new ActivityBaseAdapter(category, log, name, stopwatch);
                });
            }

            [Fact]
            public void NameIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var category = "myCategory";
                var log = Mock.Of<ILog>();
                string name = null;
                var stopwatch = Mock.Of<IStopwatch>();

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new ActivityBaseAdapter(category, log, name, stopwatch);
                });
            }

            [Fact]
            public void StopwatchIsNull_SetsStopwatchToStopwatchAdapter()
            {
                // Arrange
                var category = "myCategory";
                var log = Mock.Of<ILog>();
                var name = "myName";
                IStopwatch stopwatch = null;

                // Act
                var activity = new ActivityBaseAdapter(category, log, name, stopwatch);

                // Assert
                Assert.IsAssignableFrom<StopwatchAdapter>(activity.Stopwatch);
            }

            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var category = "myCategory";
                var log = Mock.Of<ILog>();
                var name = "myName";
                var stopwatch = Mock.Of<IStopwatch>();

                // Act
                var activity = new ActivityBaseAdapter(category, log, name, stopwatch);

                // Assert
                Assert.IsAssignableFrom<ActivityBase>(activity);
            }

            [Fact]
            public void LogsStartMessage()
            {
                // Arrange
                var category = "myCategory";
                var mockLog = new Mock<ILog>();
                mockLog.Setup(l => l.IsDebugEnabled).Returns(true);
                var log = mockLog.Object;
                var name = "myName";
                var stopwatch = Mock.Of<IStopwatch>();

                // Act
                var activity = new ActivityBaseAdapter(category, log, name, stopwatch);

                // Assert
                mockLog.Verify(l => l.Debug("Start myCategory myName"));
            }
        }

        public class DisposeMethod : ActivityBaseTests
        {
            [Fact]
            public void LogsEndMessage()
            {
                // Arrange -> Act
                activity.Dispose();

                // Assert
                mockLog.Verify(l => l.Debug("End myCategory myName"));
            }

            [Fact]
            public void DisposingMoreThanOnce_OnlyLogsEndMessageOnce()
            {
                // Arrange -> Act
                activity.Dispose();
                activity.Dispose();

                // Assert
                mockLog.Verify(l => l.Debug("End myCategory myName"), Times.Once);
            }
        }

        private class ActivityBaseAdapter : ActivityBase
        {
            public ActivityBaseAdapter(string category, ILog log, string name, IStopwatch stopwatch) : base(category, log, name, stopwatch) { }
        }
    }
}
