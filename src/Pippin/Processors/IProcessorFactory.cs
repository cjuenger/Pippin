using System;

namespace Pippin.Processors
{
    /// <summary>
    /// Factory to create instances of processors
    /// </summary>
    public interface IProcessorFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="IQueueProcessor{TItem}"/>
        /// </summary>
        /// <param name="processor">Processor handler that is use to process the queued items</param>
        /// <typeparam name="TItem">Type of items to process</typeparam>
        /// <returns></returns>
        IQueueProcessor<TItem> CreateQueueProcessor<TItem>(Action<TItem> processor);
    }
}