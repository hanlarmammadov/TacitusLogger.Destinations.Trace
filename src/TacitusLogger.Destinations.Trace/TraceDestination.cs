using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.Trace
{
    /// <summary>
    /// Destination that writes log model to System.Diagnostics.Trace.
    /// </summary>
    public class TraceDestination : NetDiagnosticsLogDestinationBase
    {
        /// <summary>
        /// Initializes a new instance of TacitusLogger.Destinations.TraceDestination class with the
        /// specified log serializer <c>ILogSerializer</c>.
        /// </summary>
        /// <param name="logSerializer">Log serializer</param>
        public TraceDestination(ILogSerializer logSerializer)
            : base(logSerializer, new TraceConsoleFacade())
        {

        }
        /// <summary>
        /// Initializes a new instance of TacitusLogger.Destinations.TraceDestination class with the
        /// default log serializer.
        /// </summary>
        public TraceDestination()
             : this(new SimpleTemplateLogSerializer())
        {

        }
        /// <summary>
        /// Initializes a new instance of TacitusLogger.Destinations.TraceDestination class with the
        /// log string template.
        /// </summary>
        /// <param name="logStringTemplate">Log string template</param>
        public TraceDestination(string logStringTemplate)
             : this(new SimpleTemplateLogSerializer(logStringTemplate))
        {

        }
        /// <summary>
        /// Initializes a new instance of TacitusLogger.Destinations.TraceDestination class with the
        /// log string factory method.
        /// </summary>
        /// <param name="logStringFactoryMethod">Log string factory method</param>
        public TraceDestination(LogModelFunc<string> logStringFactoryMethod)
             : this(new GeneratorFunctionLogSerializer(logStringFactoryMethod))
        {

        }
    }
}
