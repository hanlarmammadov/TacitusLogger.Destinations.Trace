using Moq; 
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.Trace.Examples
{
    public class Configuring
    {
        public void Creating_And_Adding_Trace_Destination_With_Default_Parameters()
        {
            TraceDestination traceDestination = new TraceDestination();
        } 
        public void Creating_And_Adding_Trace_Destination_With_Simple_Template_Log_Serializer_And_Custom_Log_Text_Template()
        {
            string template = "[$LogDate]-[$LogType]-[$Description]-[From: $Context]-[Src: $Source]-[Id: $LogId]";
            TraceDestination traceDestination = new TraceDestination(template);
            Logger logger = new Logger();
            logger.AddLogDestinations(traceDestination);
        }
        public void Creating_And_Trace_Destination_With_Extended_Template_Log_Serializer_And_Custom_Log_Text_Template()
        {
            ExtendedTemplateLogSerializer logSerializer = new ExtendedTemplateLogSerializer();
            TraceDestination traceDestination = new TraceDestination(logSerializer);
            Logger logger = new Logger();
            logger.NewLogGroup(x => true).AddDestinations(traceDestination);
        }
        public void Creating_And_Adding_Trace_Destination_With_Generator_Function_Log_Serializer()
        {
            LogModelFunc<string> logTextGenerator = log => $"{log.LogType}: {log.Description}";
            TraceDestination traceDestination = new TraceDestination(logTextGenerator);
            Logger logger = new Logger();
            logger.NewLogGroup(x => true).AddDestinations(traceDestination);
        } 
        public void Creating_And_Adding_Trace_Destination_With_Custom_Log_Serializer()
        {
            ILogSerializer customLogSerializer = new Mock<ILogSerializer>().Object;
            TraceDestination traceDestination = new TraceDestination(customLogSerializer);
        }
    }
}
