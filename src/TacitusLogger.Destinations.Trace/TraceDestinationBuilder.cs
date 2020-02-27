using System;
using System.ComponentModel;
using TacitusLogger.Builders;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.Trace
{
    /// <summary>
    /// Builds and adds an instance of <c>TacitusLogger.Destinations.TraceDestination</c> class to the <c>TacitusLogger.Builders.ILogGroupDestinationsBuilder</c>
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class TraceDestinationBuilder : LogDestinationBuilderBase, ITraceDestinationBuilder
    {
        private ILogSerializer _logSerializer;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Builders.TraceDestinationBuilder</c> using parent <c>ILogGroupDestinationsBuilder</c> instance.
        /// </summary>
        /// <param name="logGroupDestinationsBuilder">Parent log group destinations builder that will be used to complete build process.</param>
        public TraceDestinationBuilder(ILogGroupDestinationsBuilder logGroupDestinationsBuilder)
            : base(logGroupDestinationsBuilder)
        {

        }

        /// <summary>
        /// Gets the log serializer that was specified during the build process.
        /// </summary>
        public ILogSerializer LogSerializer => _logSerializer;

        /// <summary>
        /// Adds a custom log serializer of type <c>TacitusLogger.Serializers.ILogSerializer</c>
        /// that will be used to generate log model text representation. If omitted, the default log serializer
        /// of type <c>TacitusLogger.Serializers.SimpleTemplateLogSerializer</c> with the default template will be added.
        /// </summary>
        /// <param name="logSerializer"></param>
        /// <returns></returns>
        public ITraceDestinationBuilder WithCustomLogSerializer(ILogSerializer logSerializer)
        {
            if (_logSerializer != null)
                throw new InvalidOperationException("Log serializer has already been set");
            _logSerializer = logSerializer ?? throw new ArgumentNullException("logSerializer");
            return this;
        }
        /// <summary>
        /// Completes log destination build process by adding it to the parent log group destination builder.
        /// </summary>
        /// <returns></returns>
        public ILogGroupDestinationsBuilder Add()
        {
            // Deps with defaults
            if (_logSerializer == null)
                _logSerializer = new SimpleTemplateLogSerializer();

            // Adding the destination to the log group.
            return AddToLogGroup(new TraceDestination(_logSerializer));
        }
    }
}
