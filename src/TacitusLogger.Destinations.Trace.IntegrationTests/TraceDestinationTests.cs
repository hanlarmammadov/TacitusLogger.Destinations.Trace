using Moq;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TacitusLogger.Builders;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.Trace.IntegrationTests
{
    [TestFixture]
    public class TraceDestinationTests
    {
        protected void ResetFacadeForTraceDestinations(LogGroupBase logGroup, IOutputDeviceFacade consoleFacade)
        {
            foreach (ILogDestination dest in ((LogGroup)logGroup).LogDestinations)
                if (dest is TraceDestination)
                    (dest as TraceDestination).ResetConsoleFacade(consoleFacade);
        }
        protected bool IsValidGuidString(string str)
        {
            Guid guid = new Guid(str);
            return guid != Guid.Empty;
        }
        protected Mock<ILogSerializer> LogSerializerThatReturnsPredefinedString(string str)
        {
            Mock<ILogSerializer> logSerializerMock = new Mock<ILogSerializer>();
            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns(str);
            return logSerializerMock;
        }

        [Test]
        public void Logger_WithOneTraceDestinationWithDefaultsForAllLogs_SendsLogToTraceFacadeSuccessfully()
        {
            //Build log
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().Trace().Add().BuildLogger();
            //Create facade mocks 
            var traceFacadeMock = new Mock<IOutputDeviceFacade>();
            //Replace default trace facade with mock
            ResetFacadeForTraceDestinations(logger.LogGroups.First(), traceFacadeMock.Object);

            //Call Logger's method to send log
            string logId = logger.Log("Context", LogType.Event, "Description", new { });

            // Assert 
            traceFacadeMock.Verify(x => x.WriteLine(It.IsNotNull<string>()), Times.Once);
        }
        [Test]
        public void Logger_WithOneTraceDestinationWithCustomLogSerializerForAllLogs_SendsLogToTraceFacadeSuccessfully()
        {
            //Build log
            var logSerializerMocks = LogSerializerThatReturnsPredefinedString("serialized log");
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().Trace().WithCustomLogSerializer(logSerializerMocks.Object).Add().BuildLogger();
            //Create facade mocks 
            var traceFacadeMock = new Mock<IOutputDeviceFacade>();
            //Replace default trace facade with mock
            ResetFacadeForTraceDestinations(logger.LogGroups.First(), traceFacadeMock.Object);

            //Call Logger's method to send log
            string logId = logger.Log("Context", LogType.Event, "Description", new { });

            // Assert 
            traceFacadeMock.Verify(x => x.WriteLine("serialized log"), Times.Once);
        }
        [Test]
        public void Logger_WithOneTraceDestinationWithTextLogSerializerForAllLogs_SendsLogToTraceFacadeSuccessfully()
        {
            //Build log 
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().Trace().WithExtendedTemplateLogText("$Context and $Description").Add().BuildLogger();
            //Create facade mocks 
            var traceFacadeMock = new Mock<IOutputDeviceFacade>();
            //Replace default trace facade with mock
            ResetFacadeForTraceDestinations(logger.LogGroups.First(), traceFacadeMock.Object);

            //Call Logger's method to send log
            string logId = logger.Log("Context1", LogType.Event, "Description1", new { });

            // Assert 
            traceFacadeMock.Verify(x => x.WriteLine("Context1 and Description1"), Times.Once);
        }
        [Test]
        public void Logger_WithOneTraceDestinationWithGeneratorFunctionLogSerializerForAllLogs_SendsLogToTraceFacadeSuccessfully()
        {
            //Build log 
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().Trace().WithGeneratorFuncLogText(x => x.Context).Add().BuildLogger();
            //Create facade mocks 
            var traceFacadeMock = new Mock<IOutputDeviceFacade>();
            //Replace default trace facade with mock
            ResetFacadeForTraceDestinations(logger.LogGroups.First(), traceFacadeMock.Object);

            //Call Logger's method to send log
            string logId = logger.Log("Context1", LogType.Event, "Description", new { });

            // Assert 
            traceFacadeMock.Verify(x => x.WriteLine("Context1"), Times.Once);
        }
         
        #region Tests for WriteLine method

        [Test]
        public void WriteLine_WhenCalled_InvokesTraceListenersWriteLineMethod()
        {
            // Arrange
            var traceListenerMock = new Mock<TraceListener>();
            System.Diagnostics.Trace.Listeners.Clear();
            System.Diagnostics.Trace.Listeners.Add(traceListenerMock.Object);
            TraceConsoleFacade traceConsoleFacade = new TraceConsoleFacade();
            string text = "some text";

            // Act
            traceConsoleFacade.WriteLine(text);

            // Assert
            traceListenerMock.Verify(x => x.WriteLine(text), Times.Once);
        }
        [Test]
        public void WriteLine_When_Called_With_Null_Text_Does_Not_Throw()
        {
            // Arrange            
            TraceConsoleFacade traceConsoleFacade = new TraceConsoleFacade();

            // Act
            traceConsoleFacade.WriteLine(null as string);
        }

        #endregion

        #region Tests for WriteLineAsync method

        [Test]
        public async Task WriteLineAsync_WhenCalled_InvokesTraceListenersWriteLineMethod()
        {
            // Arrange
            var traceListenerMock = new Mock<TraceListener>();
            System.Diagnostics.Trace.Listeners.Clear();
            System.Diagnostics.Trace.Listeners.Add(traceListenerMock.Object);
            TraceConsoleFacade traceConsoleFacade = new TraceConsoleFacade();
            string text = "some text";

            // Act
            await traceConsoleFacade.WriteLineAsync(text);

            // Assert
            traceListenerMock.Verify(x => x.WriteLine(text), Times.Once);
        }
        [Test]
        public void WriteLineAsync_WhenCalledWithNullText_ThrowsArgumentNullException()
        {
            // Arrange
            TraceConsoleFacade traceConsoleFacade = new TraceConsoleFacade();

            Assert.CatchAsync<ArgumentNullException>(async () =>
            {
                // Act
                await traceConsoleFacade.WriteLineAsync(null);
            });
        }

        #endregion
    }
}
