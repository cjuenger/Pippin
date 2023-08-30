using System;
using System.Diagnostics.CodeAnalysis;

namespace Pippin.Processors
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage] // exclusion reason: simple factory class
    public class ProcessorFactory : IProcessorFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="processor"></param>
        /// <typeparam name="TItem"></typeparam>
        /// <returns></returns>
        public IQueueProcessor<TItem> CreateQueueProcessor<TItem>(Action<TItem> processor)
        {
            return new QueueProcessor<TItem>(processor);
        }
    }
}