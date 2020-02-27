using NUnit.Framework;
using System;
using TacitusLogger.Builders;

namespace TacitusLogger.Destinations.Trace.IntegrationTests
{
    [TestFixture]
    public class LoggerBuilderTests
    {
        [Test]
        public void Logger_With_One_Trace_Destination_When_LogSerializer_Is_Specified_Twice_Throws_InvalidOperationException()
        {
            //Build log 
            Assert.Catch<InvalidOperationException>(() =>
            {
                Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs()
                                                              .Trace()
                                                              .WithJsonLogText()
                                                              .WithExtendedTemplateLogText()
                                                              .Add()
                                                              .BuildLogger();
            });
        }
    }
}
