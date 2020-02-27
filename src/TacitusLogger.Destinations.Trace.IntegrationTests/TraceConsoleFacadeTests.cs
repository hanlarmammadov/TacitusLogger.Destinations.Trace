using Moq;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.Destinations.Trace.IntegrationTests
{
    [TestFixture]
    public class TraceConsoleFacadeTests
    {
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
        public async Task WriteLineAsync_When_Called_Invokes_Trace_Listeners_Write_Line_Method()
        {
            // Arrange
            var traceListenerMock = new Mock<TraceListener>();
            System.Diagnostics.Trace.Listeners.Clear();
            System.Diagnostics.Trace.Listeners.Add(traceListenerMock.Object);
            TraceConsoleFacade traceConsoleFacade = new TraceConsoleFacade();
            string text = "some text";
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            await traceConsoleFacade.WriteLineAsync(text, cancellationToken);

            // Assert
            traceListenerMock.Verify(x => x.WriteLine(text), Times.Once);
        }
        [Test]
        public void WriteLineAsync_When_Called_With_Cancelled_Token_Returns_Cancelled_Task()
        {
            // Arrange
            var traceListenerMock = new Mock<TraceListener>();
            System.Diagnostics.Trace.Listeners.Clear();
            System.Diagnostics.Trace.Listeners.Add(traceListenerMock.Object);
            TraceConsoleFacade traceConsoleFacade = new TraceConsoleFacade();
            string text = "some text";
            CancellationToken cancellationToken = new CancellationToken(canceled: true);

            Assert.CatchAsync<OperationCanceledException>(async () =>
            {
                // Act
                await traceConsoleFacade.WriteLineAsync(text, cancellationToken);
            });

            // Assert
            traceListenerMock.Verify(x => x.WriteLine(text), Times.Never);
        }
        [Test]
        public void WriteLineAsync_When_Called_With_Null_Text_Throws_ArgumentNullException()
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
