using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Serializers;
using TacitusLogger.Tests.Helpers;

namespace TacitusLogger.Destinations.Trace.UnitTests
{
    [TestFixture]
    public class TraceDestinationTests
    {
        #region Ctors tests

        [Test]
        public void Ctor_WithLogSerializer_WhenCalled_LogSerializerAndConsoleFacadeAreSetRight()
        {
            // Arrange
            ILogSerializer logSerializer = new Mock<ILogSerializer>().Object;

            // Act
            TraceDestination traceDestination = new TraceDestination(logSerializer);

            // Assert  
            Assert.AreEqual(logSerializer, traceDestination.LogSerializer);
            Assert.IsInstanceOf<TraceConsoleFacade>(traceDestination.ConsoleFacade);
        }

        [Test]
        public void Ctor_WithLogSerializer_WhenCalledWithNullLogSerializer_ThrowsArgumentNullException()
        {

            // Assert  
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                TraceDestination traceDestination = new TraceDestination(null as ILogSerializer);
            });
        }

        [Test]
        public void Ctor_Default_WhenCalled_LogSerializerIsSetToDefault()
        {
            // Act
            TraceDestination traceDestination = new TraceDestination();

            // Assert  
            Assert.IsInstanceOf<SimpleTemplateLogSerializer>(traceDestination.LogSerializer);
            Assert.IsInstanceOf<TraceConsoleFacade>(traceDestination.ConsoleFacade);
        }

        [Test]
        public void Ctor_WithLogStringTemplate_WhenCalled_LogSerializerIsSetToTextLogSerializerWithProvidedTemplate()
        {
            // Arrange
            var logTemplate = "sampleLogTemplate";

            // Act
            TraceDestination traceDestination = new TraceDestination(logTemplate);

            // Assert  
            Assert.IsInstanceOf<SimpleTemplateLogSerializer>(traceDestination.LogSerializer);
            Assert.AreEqual(logTemplate, (traceDestination.LogSerializer as SimpleTemplateLogSerializer).Template);
            Assert.IsInstanceOf<TraceConsoleFacade>(traceDestination.ConsoleFacade);
        }

        [Test]
        public void Ctor_WithLogStringTemplate_WhenCalledWithNullLogStringTemplate_ThrowsArgumentIsNullException()
        {
            // Assert  
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                TraceDestination traceDestination = new TraceDestination(null as string);
            });
        }

        [Test]
        public void Ctor_WithLogModelFunc_WhenCalled_LogSerializerIsSetToGeneratorFunctionLogSerializerWithProvidedDeligate()
        {
            // Arrange
            LogModelFunc<string> generatorFunc = (ld) => "";

            // Act
            TraceDestination traceDestination = new TraceDestination(generatorFunc);

            // Assert  
            Assert.IsInstanceOf<GeneratorFunctionLogSerializer>(traceDestination.LogSerializer);
            Assert.AreEqual(generatorFunc, (traceDestination.LogSerializer as GeneratorFunctionLogSerializer).GeneratorFunction);
            Assert.IsInstanceOf<TraceConsoleFacade>(traceDestination.ConsoleFacade);
        }

        [Test]
        public void Ctor_WithLogModelFunc_WhenCalledWithNullDelegate_ThrowsArgumentIsNullException()
        {
            // Assert  
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                TraceDestination traceDestination = new TraceDestination(null as LogModelFunc<string>);
            });
        }

        #endregion

        #region ResetConsole tests
        [Test]
        public void ResetConsole_WhenCalled_ResetsConsoleFacadeFromDefaultToProvided()
        {
            // Arrange
            var consoleFacadeMock = new Mock<IOutputDeviceFacade>().Object;

            // Act

            TraceDestination traceDestination = new TraceDestination();
            traceDestination.ResetConsoleFacade(consoleFacadeMock);

            // Assert  
            Assert.AreEqual(consoleFacadeMock, traceDestination.ConsoleFacade);
        }
        #endregion

        #region Send tests

        [Test]
        public void Send_WhenCalled_ProvidesConsoleFacadeWithGeneratedLogText()
        {
            // Arrange
            var consoleFacadeMock = new Mock<IOutputDeviceFacade>();
            var logSerializer = new Mock<ILogSerializer>();

            var logModel = Samples.LogModels.Standard(LogType.Info);

            logSerializer.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("resulting text");

            TraceDestination traceDestination = new TraceDestination(logSerializer.Object);
            traceDestination.ResetConsoleFacade(consoleFacadeMock.Object);

            // Act
            traceDestination.Send(new LogModel[] { logModel });

            // Assert  
            consoleFacadeMock.Verify(x => x.WriteLine("resulting text"), Times.Once);
        }

        [Test]
        public void Send_WhenCalled_NoneOfDependantsAsyncMethodsAreCalled()
        {
            // Arrange
            var consoleFacadeMock = new Mock<IOutputDeviceFacade>();
            var logModel = Samples.LogModels.Standard(LogType.Info);
            TraceDestination traceDestination = new TraceDestination();
            traceDestination.ResetConsoleFacade(consoleFacadeMock.Object);

            // Act
            traceDestination.Send(new LogModel[] { logModel });

            // Assert  
            consoleFacadeMock.Verify(x => x.WriteLineAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(15)]
        public void Send_When_Called_With_Collection_Of_N_Logs_Calls_Facade_Method_N_Times(int N)
        {
            // Arrange
            var traceConsoleFacadeMock = new Mock<IOutputDeviceFacade>();
            var logSerializerMock = new Mock<ILogSerializer>();
            var logs = new LogModel[N];
            for (int i = 0; i < N; i++)
                logs[i] = new LogModel() { Description = $"logText{i}" };

            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns((LogModel x) => x.Description);
            TraceDestination traceDestination = new TraceDestination(logSerializerMock.Object);
            traceDestination.ResetConsoleFacade(traceConsoleFacadeMock.Object);

            // Act
            traceDestination.Send(logs);

            // Assert  
            traceConsoleFacadeMock.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(N));
            for (int i = 0; i < N; i++)
                traceConsoleFacadeMock.Verify(x => x.WriteLine($"logText{i}"), Times.Once);
        }

        #endregion

        #region SendAsync tests

        [Test]
        public async Task SendAsync_When_Called_Provides_Console_Facade_With_Generated_Log_Text()
        {
            // Arrange
            var logModel = Samples.LogModels.Standard(LogType.Info);
            var consoleFacadeMock = new Mock<IOutputDeviceFacade>();
            var logTextSerializerMock = new Mock<ILogSerializer>();
            logTextSerializerMock.Setup(x => x.Serialize(logModel)).Returns("resulting text");
            TraceDestination traceDestination = new TraceDestination(logTextSerializerMock.Object);
            traceDestination.ResetConsoleFacade(consoleFacadeMock.Object);
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            await traceDestination.SendAsync(new LogModel[] { logModel }, cancellationToken);

            // Assert  
            consoleFacadeMock.Verify(x => x.WriteLineAsync("resulting text", cancellationToken), Times.Once);
        }

        [Test]
        public void SendAsync_When_Log_Serializer_Returns_Null_Throws_An_Exception()
        {
            // Arrange
            var consoleFacadeMock = new Mock<IOutputDeviceFacade>();
            var logSerializer = new Mock<ILogSerializer>();
            logSerializer.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns(null as string);
            var logModel = Samples.LogModels.Standard(LogType.Info);
            TraceDestination traceDestination = new TraceDestination(logSerializer.Object);
            traceDestination.ResetConsoleFacade(consoleFacadeMock.Object);

            // Assert
            Assert.CatchAsync<Exception>(async () =>
            {
                // Act
                await traceDestination.SendAsync(new LogModel[] { logModel });
            });
        }

        [Test]
        public async Task SendAsync_When_Called_None_Of_Dependants_Sync_Methods_Are_Called()
        {
            // Arrange
            var consoleFacadeMock = new Mock<IOutputDeviceFacade>();
            var logModel = Samples.LogModels.Standard(LogType.Info);
            TraceDestination traceDestination = new TraceDestination();
            traceDestination.ResetConsoleFacade(consoleFacadeMock.Object);

            // Act
            await traceDestination.SendAsync(new LogModel[] { logModel });

            // Assert
            consoleFacadeMock.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Never);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(15)]
        public async Task SendAsync_When_Called_With_Collection_Of_N_Logs_Calls_Facade_Method_N_Times(int N)
        {
            // Arrange
            var traceConsoleFacadeMock = new Mock<IOutputDeviceFacade>();
            var logSerializerMock = new Mock<ILogSerializer>();
            var logs = new LogModel[N];
            for (int i = 0; i < N; i++)
                logs[i] = new LogModel() { Description = $"logText{i}" };

            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns((LogModel x) => x.Description);
            TraceDestination traceDestination = new TraceDestination(logSerializerMock.Object);
            traceDestination.ResetConsoleFacade(traceConsoleFacadeMock.Object);
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            await traceDestination.SendAsync(logs, cancellationToken);

            // Assert  
            traceConsoleFacadeMock.Verify(x => x.WriteLineAsync(It.IsAny<string>(), cancellationToken), Times.Exactly(N));
            for (int i = 0; i < N; i++)
                traceConsoleFacadeMock.Verify(x => x.WriteLineAsync($"logText{i}", cancellationToken), Times.Once);
        }

        [Test]
        public void SendAsync_When_Called_With_Cancelled_Cancellation_Token_Immediately_Returns_Cancelled_Task()
        {
            // Arrange
            var consoleFacadeMock = new Mock<IOutputDeviceFacade>();
            var logSerializer = new Mock<ILogSerializer>();
            TraceDestination traceDestination = new TraceDestination(logSerializer.Object);
            traceDestination.ResetConsoleFacade(consoleFacadeMock.Object);
            CancellationToken cancellationToken = new CancellationToken(canceled: true);

            // Assert  
            Assert.CatchAsync<TaskCanceledException>(async () =>
            {
                // Act
                await traceDestination.SendAsync(new LogModel[] { Samples.LogModels.Standard(LogType.Info) }, cancellationToken);
            });
            logSerializer.Verify(x => x.Serialize(It.IsAny<LogModel>()), Times.Never);
            consoleFacadeMock.Verify(x => x.WriteLineAsync(It.IsAny<String>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion

        #region Tests for ToString method

        [Test]
        public void ToString_When_Called_Returns_Information_About_The_Destination()
        {
            // Arrange
            TraceDestination traceDestination = new TraceDestination();

            // Act
            var result = traceDestination.ToString();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("TacitusLogger.Destinations.Trace.TraceDestination"));
        }
        [Test]
        public void ToString_When_Called_Calls_ToString_Method_Of_Log_Serializer()
        {
            // Arrange
            var logSerializerMock = new Mock<ILogSerializer>();
            var logSerializerDescription = "logSerializerDescription";
            logSerializerMock.Setup(x => x.ToString()).Returns(logSerializerDescription);
            TraceDestination traceDestination = new TraceDestination(logSerializerMock.Object);

            // Act
            var result = traceDestination.ToString();

            // Arrange
            logSerializerMock.Verify(x => x.ToString(), Times.Once);
            Assert.IsTrue(result.Contains(logSerializerDescription));
        }

        #endregion
    }
}
