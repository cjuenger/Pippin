using System;

namespace Pippin.Filters
{
    /// <summary>
    /// A thread-safe concurrent filter that has input and output.
    /// </summary>
    /// <typeparam name="TInput">Type of the input</typeparam>
    /// <typeparam name="TOutput">Type of the output</typeparam>
    public abstract class ConcurrentFilter<TInput, TOutput> : ConcurrentFilterOutput<TOutput>, IConcurrentFilter<TInput, TOutput>
    {
        /// <inheritdoc />
        public void Input(TInput input)
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
    }
}