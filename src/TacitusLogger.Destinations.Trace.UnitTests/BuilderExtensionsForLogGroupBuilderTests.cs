using Moq;
using NUnit.Framework; 
using TacitusLogger.Builders;

namespace TacitusLogger.Destinations.Trace.UnitTests
{
    [TestFixture]
    public class BuilderExtensionsForLogGroupBuilderTests
    { 
        #region Tests for Trace extension method

        [Test]
        public void Trace_Taking_LogGroupDestinationsBuilder_WhenCalled_Returns_TraceDestinationBuilder_With_LogGroupDestinationsBuilder_Set()
        {
            // Arrange
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;

            // Act
            ITraceDestinationBuilder traceDestinationBuilder = BuilderExtensionsForLogGroupBuilder.Trace(logGroupDestinationsBuilder);

            // Assert
            Assert.IsInstanceOf<TraceDestinationBuilder>(traceDestinationBuilder);
            Assert.AreEqual(logGroupDestinationsBuilder, (traceDestinationBuilder as TraceDestinationBuilder).LogGroupDestinationsBuilder);
        }

        #endregion 
    }
}
