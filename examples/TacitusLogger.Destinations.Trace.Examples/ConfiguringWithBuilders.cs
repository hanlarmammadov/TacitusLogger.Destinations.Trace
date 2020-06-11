using Moq; 
using TacitusLogger.Builders;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.Trace.Examples
{
    public class ConfiguringWithBuilders
    {  
        public void Adding_Trace_Destination_With_Default_Parameters()
        {
            ILogger logger = LoggerBuilder.Logger().ForAllLogs()
                                                   .Trace().Add()
                                                   .BuildLogger();
        }
        public void Adding_Trace_Destination_With_Simple_Template_Log_Serializer_And_Custom_Log_Text_Template()
        {
            ILogger logger = LoggerBuilder.Logger()
                                          .ForAllLogs()
                                          .Trace().WithSimpleTemplateLogText("[$LogDate]-[$LogType]-[$Description]-[From: $Context]-[Src: $Source]-[Id: $LogId]")
                                                  .Add()
                                          .BuildLogger();
        }
        public void Adding_Trace_Destination_With_Extended_Template_Log_Serializer_And_Custom_Log_Text_Template()
        {
            string template = Templates.Extended.Default;

            ILogger logger = LoggerBuilder.Logger()
                                          .ForAllLogs()
                                          .Trace().WithExtendedTemplateLogText(template)
                                                  .Add()
                                          .BuildLogger();
        }
        public void Adding_Trace_Destination_With_Generator_Function_Log_Serializer()
        {
            ILogger logger = LoggerBuilder.Logger()
                                          .ForAllLogs()
                                          .Trace().WithGeneratorFuncLogText(log => $"{log.LogType}: {log.Description}")
                                                  .Add()
                                          .BuildLogger();
        }
        public void Adding_Trace_Destination_With_Custom_Log_Serializer()
        {
            ILogSerializer customLogSerializer = new Mock<ILogSerializer>().Object;

            ILogger logger = LoggerBuilder.Logger()
                                          .ForAllLogs()
                                          .Trace().WithCustomLogSerializer(customLogSerializer)
                                                  .Add()
                                          .BuildLogger();
        }
    }
}
