using System.Diagnostics.CodeAnalysis;

namespace Pippin.Sequences
{
    /// <summary>
    /// A builder for consecutive sequences of payload data.
    /// For each sequence block a global unique ID common for all sequences of this builder will be passed to a created sequence.
    /// Also each built sequence block is assigned a consecutive serial number.
    /// </summary>
    /// <typeparam name="TPayload">The actual payload of the sequence.</typeparam>
    public interface ISequenceBlockBuilder<TPayload>
    {
        /// <summary>
        /// Builds a new <see cref="SequenceBlock{TPayload}"/> with the passed payload.
        /// </summary>
        /// <param name="payload">The sequence block payload.</param>
        /// <returns></returns>
        SequenceBlock<TPayload> Build(TPayload payload);
    }
}