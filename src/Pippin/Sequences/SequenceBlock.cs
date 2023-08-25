using System;

namespace Pippin.Sequences
{
    /// <summary>
    /// A single wrapper block of a cohesive sequence.
    /// </summary>
    /// <typeparam name="TPayload">Type of the payload data</typeparam>
    public class SequenceBlock<TPayload>
    {
        /// <summary>
        /// Unique ID of a cohesive sequence
        /// </summary>
        public Guid UniqueSequenceId { get; }
        
        /// <summary>
        /// Consecutive serial number of single sequence block
        /// </summary>
        public long SerialNumber { get; }
        
        /// <summary>
        /// The actual payload of the sequence block
        /// </summary>
        public TPayload Payload { get; }

        /// <summary>
        /// Initializes an instance of <see cref="SequenceBlock{TPayload}"/>
        /// </summary>
        /// <param name="uniqueSequenceId">Unique ID of a cohesive sequence</param>
        /// <param name="serialNumber">Consecutive serial number of single sequence block</param>
        /// <param name="payload">The actual payload of the sequence block</param>
        /// <exception cref="ArgumentNullException">The argument <see cref="payload"/> is null</exception>
        public SequenceBlock(Guid uniqueSequenceId, long serialNumber, TPayload payload)
        {
            UniqueSequenceId = uniqueSequenceId;
            SerialNumber = serialNumber;
            Payload = payload ?? throw new ArgumentNullException(nameof(payload));
        }
    }
}