using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.Destinations.Trace
{
    /// <summary>
    /// A facade for System.Diagnostics.Trace.
    /// This class is internal.
    /// </summary>
    internal class TraceConsoleFacade : IOutputDeviceFacade
    {
        /// <summary>
        /// Writes the <paramref name="text"/> to the <c>System.Diagnostics.Trace.</c>
        /// </summary>
        /// <param name="text"></param>
        public void WriteLine(string text)
        {
            System.Diagnostics.Trace.WriteLine(text);
        }
        /// <summary>
        /// Asynchronously writes the <paramref name="text"/> to the <c>System.Diagnostics.Trace.</c>
        /// </summary>
        /// <param name="text"></param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task WriteLineAsync(string text, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
                await Task.FromCanceled(cancellationToken);
            if (text == null)
                throw new ArgumentNullException("text");
            System.Diagnostics.Trace.WriteLine(text);
            await Task.CompletedTask;
        }
    }
}
