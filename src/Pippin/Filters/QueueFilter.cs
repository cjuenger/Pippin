using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Pippin.Processors;

namespace Pippin.Filters
{
    /// <summary>
    /// A thread-safe filter that queues the input within a <see cref="IQueueProcessor{TItem}"/> which
    /// processes the queued input items on a background thread.
    /// </summary>
    /// <typeparam name="TInput">Type of the input</typeparam>
    /// <typeparam name="TOutput">Type of the output</typeparam>
    public abstract class QueueFilter<TInput, TOutput> : Filter<TInput, TOutput>, IDisposable
    {
        private readonly IQueueProcessor<TInput> _queueProcessor;
        
        /// <summary>
        /// Creates an instance of <see cref="QueueFilter{TInput,TOutput}"/>
        /// </summary>
        /// <param name="processorFactory">Factory used to create instance of <see cref="IQueueProcessor{TItem}"/></param>
        protected QueueFilter(IProcessorFactory? processorFactory)
        {
            processorFactory ??= new ProcessorFactory();
            _queueProcessor = processorFactory.CreateQueueProcessor<TInput>(ProcessDequeuedItem);
        }
        
        /// <inheritdoc />
        public override void Input(TInput input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            _queueProcessor.Enqueue(input);
        }

        private void ProcessDequeuedItem(TInput input)
        {
            foreach (var output in PipePlugs.Select(pipePlug => Process(input)))
            {
                Output(output);
            }
        }
        
        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            _queueProcessor.Dispose();
        }
    }
}