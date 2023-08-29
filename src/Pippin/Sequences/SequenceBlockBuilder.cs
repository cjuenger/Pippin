using System;
using System.Diagnostics.CodeAnalysis;

namespace Pippin.Sequences
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public class SequenceBlockBuilder<TPackageData> : ISequenceBlockBuilder<TPackageData>
    {
        private readonly Guid _uniqueSequenceId;
        private long _serialNumber;
        
        
        /// <summary>
        /// Creates an instance of <see cref="SequenceBlockBuilder{TPackageData}"/> using a random GUID.
        /// </summary>
        public SequenceBlockBuilder() : this(Guid.NewGuid()) {}
        
        /// <summary>
        /// Creates an instance of <see cref="SequenceBlockBuilder{TPackageData}"/> using a passed GUID.
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