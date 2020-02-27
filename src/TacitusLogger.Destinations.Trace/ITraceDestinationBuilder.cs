using System.ComponentModel; 
using TacitusLogger.Builders;

namespace TacitusLogger.Destinations.Trace
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ITraceDestinationBuilder : IDestinationBuilder, IBuilderWithLogTextSerialization<ITraceDestinationBuilder>
    {

    }
}
