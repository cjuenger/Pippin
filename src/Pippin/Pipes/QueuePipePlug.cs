using System;
using Pippin.Processors;

namespace Pippin.Pipes
{
    /// <summary>
    /// A thread-safe variant of <see cref="PipePlug{TInput}"/> which queues the input
    /// within a <see cref="IQueueProcessor{TItem}"/> which processes the queued input items on a background thread.
    /// </summary>
    /// <typeparam name="TInput">Type of the input</typeparam>
    public abstract class QueuePipePlug<TInput> : PipePlug<TInput>
    {
        private readonly IQueueProcessor<TInput> _queueProcessor;
        
        /// <summary>
        /// Creates an instance of <see cref="QueuePipePlug{TInput}"/>
        /// </summary>
        /// <param name="processorFactory">Factory used to create instance of <see cref="IQueueProcessor{TItem}"/></param>
        protected QueuePipePlug(IProcessorFactory? processorFactory)
        {
            processorFactory ??= new ProcessorFactory();
            _queueProcessor = processorFactory.CreateQueueProcessor<TInput>(ProcessInput);
        }

        /// <inheritdoc />
        public override void Input(TInput input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            _queueProcessor.Enqueue(input);
        }
    }
}