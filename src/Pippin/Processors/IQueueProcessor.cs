using System;

namespace Pippin.Processors
{
    /// <summary>
    /// Queues items to be processed on a background thread. 
    /// </summary>
    /// <typeparam name="TItem">Generic type of the enqueued item</typeparam>
    public interface IQueueProcessor<in TItem> : IDisposable
    {
        /// <summary>
        /// Enqueues an item to be processed on a background thread.
        /// </summary>
        /// <param name="item"></param>
        void Enqueue(TItem item);
    }
}