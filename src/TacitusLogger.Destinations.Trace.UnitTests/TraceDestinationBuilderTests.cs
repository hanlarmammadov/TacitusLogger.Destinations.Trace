using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TacitusLogger.Builders;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.Trace.UnitTests
{
    [TestFixture]
    public class TraceDestinationBuilderTests
    {
        #region Tests for Ctor

        [Test]
        public void Ctor_WithLogGroupDestinationsBuilder_WhenCalled_InitializesLogGroupDestinationsBuilder()
        {
            // Arrange
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;

            // Act
            TraceDestinationBuilder traceDestinationBuilder = new TraceDestinationBuilder(logGroupDestinationsBuilder);

            // Assert
            Assert.AreEqual(logGroupDestinationsBuilder, traceDestinationBuilder.LogGroupDestinationsBuilder);
        }

        #endregion

        #region Tests for WithCustomLogSerializer method

        [Test]
        public void WithCustomLogSerializer_WhenCalled_SetsLogSerializer()
        {
            // Arrange
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            TraceDestinationBuilder traceDestinationBuilder = new TraceDestinationBuilder(logGroupDestinationsBuilder);
            ILogSerializer logSerializer = new Mock<ILogSerializer>().Object;

            // Act
            traceDestinationBuilder.WithCustomLogSerializer(logSerializer);

            // Assert
            Assert.AreEqual(logSerializer, traceDestinationBuilder.LogSerializer);
        }

        [Test]
        public void WithCustomLogSerializer_WhenCalled_ReturnsTraceDestinationBuilder()
        {
            // Arrange
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            TraceDestinationBuilder traceDestinationBuilder = new TraceDestinationBuilder(logGroupDestinationsBuilder);
            ILogSerializer logSerializer = new Mock<ILogSerializer>().Object;

            // Act
            ITraceDestinationBuilder returned = traceDestinationBuilder.WithCustomLogSerializer(logSerializer);

            // Assert
            Assert.AreEqual(traceDestinationBuilder, returned);
        }

        [Test]
        public void WithCustomLogSerializer_When_Called_Given_That_Already_Set_Throws_InvalidOperationException()
        {
            // Arrange
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            TraceDestinationBuilder traceDestinationBuilder = new TraceDestinationBuilder(logGroupDestinationsBuilder);
            // Set first time.
            traceDestinationBuilder.WithCustomLogSerializer(new Mock<ILogSerializer>().Object);

            // Assert
            Assert.Catch<InvalidOperationException>(() =>
            {
                // Tried to set second time.
                traceDestinationBuilder.WithCustomLogSerializer(new Mock<ILogSerializer>().Object);
            });
        }

        [Test]
        public void WithCustomLogSerializer_WhenCalledWithNullLogSerializer_ThrowsArgumentNullException()
        {
            // Arrange
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            TraceDestinationBuilder traceDestinationBuilder = new TraceDestinationBuilder(logGroupDestinationsBuilder);

            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                traceDestinationBuilder.WithCustomLogSerializer(null);
            });
        }

        #endregion

        #region Tests for Add method

        [Test]
        public void Add_WhenCalled_ReturnsLogGroupDestinationsBuilder()
        {
            // Arrange
            var loggerBuilder = LoggerBuilder.Logger();
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = loggerBuilder.NewLogGroup().ForAllLogs();
            TraceDestinationBuilder traceDestinationBuilder = new TraceDestinationBuilder(logGroupDestinationsBuilder);

            // Act
            ILogGroupDestinationsBuilder logGroupDestinationsBuilderReturned = traceDestinationBuilder.Add();

            // Assert
            Assert.AreEqual(logGroupDestinationsBuilder, logGroupDestinationsBuilderReturned);
        }

        [Test]
        public void Add_WhenCalled_AddsNewTraceDestinationToLogGroupDestinationsBuilder()
        {
            // Arrange 
            var logGroupDestinationsBuilderMock = new Mock<ILogGroupDestinationsBuilder>();
            TraceDestinationBuilder traceDestinationBuilder = new TraceDestinationBuilder(logGroupDestinationsBuilderMock.Object);

            // Act
            traceDestinationBuilder.Add();

            // Assert
            logGroupDestinationsBuilderMock.Verify(x => x.CustomDestination(It.IsNotNull<TraceDestination>()), Times.Once);
        }

        [Test]
        public void Add_WhenCalledGivenThatLogSerializerWasProvided_ReturnsLogGroupDestinationsBuilderWithSetLogSerializer()
        {
            // Arrange
            var loggerBuilder = LoggerBuilder.Logger();
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = loggerBuilder.NewLogGroup().ForAllLogs();
            TraceDestinationBuilder traceDestinationBuilder = new TraceDestinationBuilder(logGroupDestinationsBuilder);
            ILogSerializer logSerializer = new Mock<ILogSerializer>().Object;
            traceDestinationBuilder.WithCustomLogSerializer(logSerializer);

            // Act
            traceDestinationBuilder.Add();

            // Assert
            Assert.AreEqual(logSerializer, traceDestinationBuilder.LogSerializer);
        }

        [Test]
        public void Add_WhenCalledGivenThatLogSerializerWasNotProvided_ReturnsLogGroupDestinationsBuilderWithDefaultLogSerializer()
        {
            // Arrange
            var loggerBuilder = LoggerBuilder.Logger();
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = loggerBuilder.NewLogGroup().ForAllLogs();
            TraceDestinationBuilder traceDestinationBuilder = new TraceDestinationBuilder(logGroupDestinationsBuilder);

            // Act
            traceDestinationBuilder.Add();

            // Assert
            Assert.IsInstanceOf<SimpleTemplateLogSerializer>(traceDestinationBuilder.LogSerializer);
        }
        #endregion
    }
}
