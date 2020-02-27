using System.ComponentModel;
using TacitusLogger.Builders;

namespace TacitusLogger.Destinations.Trace
{
    /// <summary>
    /// Adds extension methods to <c>TacitusLogger.Builders.ILogGroupDestinationsBuilder</c> interface and its implementations.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class BuilderExtensionsForLogGroupBuilder
    {
        /// <summary>
        /// Initiate the adding a trace destination to the log group builder.
        /// </summary>
        /// <param name="obj">Log group destination builder.</param>
        /// <returns>Trace destination builder.</returns>
        public static ITraceDestinationBuilder Trace(this ILogGroupDestinationsBuilder obj)
        {
            return new TraceDestinationBuilder(obj);
        }
    }
}
