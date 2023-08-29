using System;
using System.Collections.Generic;

namespace Pippin.Filters
{
    /// <summary>
    /// A filter that has input and output.
    /// </summary>
    /// <typeparam name="TInput">Type of the input</typeparam>
    /// <typeparam name="TOutput">Type of the output</typeparam>
    public abstract class Filter<TInput, TOutput> : IFilter<TInput, TOutput>
    {
        protected readonly List<IFilterInput<TOutput>> Filters = new List<IFilterInput<TOutput>>();

        /// <inheritdoc />
        public void ChainFilter(IFilterInput<TOutput> filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            Filters.Add(filter);
        }
        
        /// <inheritdoc />
        public virtual void Input(TInput input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            var output = Filtrate(input);
            Output(output);
        }

        /// <summary>
        /// Contract for the filtration
        /// The term 'Filtrate' is chosen to be close to the context of filtering.
        /// It should implement the processing that shall be performed on the input.
        /// </summary>
        /// <param name="input">Input data</param>
        /// <returns>Returns the filtered/processed output</returns>
        protected abstract TOutput Filtrate(TInput input);
        
        /// <summary>
        /// Output to all chained filters
        /// </summary>
        /// <param name="output">Output data</param>
        /// <exception cref="ArgumentNullException">The passed argument 'output' is null.</exception>
        protected void Output(TOutput output)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));
            foreach (var filter in Filters) filter.Input(output);
        }
    }
}