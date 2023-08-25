using System;

namespace Pippin.Sequences
{
    /// <inheritdoc />
    public class SequenceBlockBuilder<TPackageData> : ISequenceBlockBuilder<TPackageData>
    {
        private readonly Guid _uniqueSequenceId;
        private long _serialNumber;

        /// <summary>
        /// Creates an instance of <see cref="SequenceBlockBuilder{TPackageData}"/>
        /// </summary>
        /// <param name="uniqueSequenceId">A unique sequence ID which identifies the coherency of each sequence block</param>
        public SequenceBlockBuilder(Guid uniqueSequenceId)
        {
            _uniqueSequenceId = uniqueSequenceId;
        }

        /// <inheritdoc />
        public SequenceBlock<TPackageData> Build(TPackageData payload)
        {
            if (payload == null) throw new ArgumentNullException(nameof(payload));
            return new SequenceBlock<TPackageData>(_uniqueSequenceId, _serialNumber++, payload);
        }
    }
}