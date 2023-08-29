using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pippin.Filters
{
    /// <summary>
    /// A thread-safe concurrent filter that queues the input within a <see cref="ConcurrentQueue{T}"/> and
    /// processes these queued input items on a background thread.
    /// </summary>
    /// <typeparam name="TInput">Type of the input</typeparam>
    /// <typeparam name="TOutput">Type of the output</typeparam>
    public abstract class QueueFilter<TInput, TOutput> : Filter<TInput, TOutput>, IDisposable
    {
        private readonly ConcurrentQueue<TInput> _queue = new ConcurrentQueue<TInput>();
        private readonly SemaphoreSlim _queueSemaphore = new SemaphoreSlim(0);
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private Exception? _exception;

        /// <summary>
        /// 
        /// </summary>
        protected QueueFilter()
        {
            Task.Factory.StartNew(() => Dequeue(_cancellationTokenSource.Token));
        }
        
        /// <inheritdoc />
        public override void Input(TInput input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            Enqueue(input);
        }
        
        private void Enqueue(TInput item)
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
                    if (!_queue.TryDequeue(out var input)) continue;
                    foreach (var output in Filters.Select(filterInput => Filtrate(input)))
                    {
                        Output(output);
                    }
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