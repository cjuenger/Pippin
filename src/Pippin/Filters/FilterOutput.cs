using System;
using System.Collections.Generic;

namespace Pippin.Filters
{
    /// <inheritdoc />
    public abstract class FilterOutput<TOutput> : IFilterOutput<TOutput>
    {
        private readonly List<IFilterInput<TOutput>> _filters = new List<IFilterInput<TOutput>>();

        /// <inheritdoc />
        public void ChainFilter(IFilterInput<TOutput> filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            _filters.Add(filter);
        }
        
        /// <summary>
        ///     Output to all chained filters
        /// </summary>
        /// <param name="output">Output data</param>
        /// <exception cref="ArgumentNullException">The passed <see cref="output"/> is null.</exception>
        protected void Output(TOutput output)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));
            foreach (var filter in _filters) filter.Input(output);
        }
    }
}