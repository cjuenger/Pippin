using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pippin.Filters
{
    public abstract class ConcurrentFilterOutput<TOutput> : IConcurrentFilterOutput<TOutput> 
    {
        private readonly ConcurrentQueue<TOutput> _queue = new ConcurrentQueue<TOutput>();
        private readonly SemaphoreSlim _queueSemaphore = new SemaphoreSlim(0);
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly List<IFilterInput<TOutput>> _filters = new List<IFilterInput<TOutput>>();
        private Exception? _exception;
        
        protected ConcurrentFilterOutput()
        {
            Task.Factory.StartNew(() => Dequeue(_cancellationTokenSource.Token));
        }
        
        public void ChainFilter(IFilterInput<TOutput> filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            _filters.Add(filter);
        }
        
        protected void Output(TOutput output)
        {
            if (_exception != null) throw _exception;
            if (output == null) throw new ArgumentNullException(nameof(output));
            Enqueue(output);
        }
        
        private void Enqueue(TOutput output)
        {
            _queue.Enqueue(output);
            _queueSemaphore.Release();
        }
        
        private void Dequeue(CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    _queueSemaphore.Wait(cancellationToken);
                    if (!_queue.TryDequeue(out var output)) continue;
                    foreach (var filter in _filters) filter.Input(output);
                }
            }
            catch (Exception exception)
            {
                _exception = exception;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _queueSemaphore.Dispose();
            _cancellationTokenSource.Dispose();
        }
    }
}