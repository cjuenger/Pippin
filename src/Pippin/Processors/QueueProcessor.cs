using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Pippin.Processors
{
    /// <inheritdoc />
    public class QueueProcessor<TItem> : IQueueProcessor<TItem>
    {
        private readonly ConcurrentQueue<TItem> _queue = new ConcurrentQueue<TItem>();
        private readonly SemaphoreSlim _queueSemaphore = new SemaphoreSlim(0);
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private Exception _exception;
        private readonly Action<TItem> _process;

        /// <summary>
        /// Creates an instance of <see cref="QueueProcessor{TItem}"/>
        /// </summary>
        /// <param name="process">Processor of the queued items</param>
        /// <exception cref="ArgumentNullException">Argument 'process' is null</exception>
        public QueueProcessor(Action<TItem> process)
        {
            _process = process ?? throw new ArgumentNullException(nameof(process));
            Task.Factory.StartNew(() => Dequeue(_cancellationTokenSource.Token));
        }

        /// <inheritdoc />
        public void Enqueue(TItem item)
        {
            if (_exception != null) throw _exception;
            _queue.Enqueue(item);
            _queueSemaphore.Release();
        }
        
        private void Dequeue(CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    _queueSemaphore.Wait(cancellationToken);
                    if (!_queue.TryDequeue(out var item)) continue;
                    _process.Invoke(item);
                }
            }
            catch (Exception exception)
            {
                _exception = exception;
            }
        }
        
        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            _queueSemaphore.Dispose();
            _cancellationTokenSource.Dispose();
        }
    }
}