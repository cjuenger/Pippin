using System;
using Pippin.Processors;

namespace Pippin.Pipes
{
    /// <summary>
    /// A thread-safe variant of <see cref="PipeSocket{TOutput}"/> which queues the output
    /// within a <see cref="IQueueProcessor{TItem}"/> which processes the queued input items on a background thread.
    /// </summary>
    /// <typeparam name="TOutput">Type of the output</typeparam>
    public abstract class QueuePipeSocket<TOutput> : PipeSocket<TOutput>
    {
        private readonly IQueueProcessor<TOutput> _queueProcessor;

        /// <summary>
        /// 
        /// </summary>
        protected QueuePipeSocket(IProcessorFactory processorFactory)
        {
            processorFactory = processorFactory ?? new ProcessorFactory();
            _queueProcessor = processorFactory.CreateQueueProcessor<TOutput>(base.Output);
        }
        
        /// <summary>
        /// Output to all chained filters
        /// </summary>
        /// <param name="output">Output data</param>
        /// <exception cref="ArgumentNullException">The passed argument 'output' is null.</exception>
        protected override void Output(TOutput output)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));
            _queueProcessor.Enqueue(output);
        }
    }
}