namespace Pippin.Filters
{
    /// <summary>
    /// A filter that has input and output.
    /// </summary>
    /// <typeparam name="TInput">Type of the input</typeparam>
    /// <typeparam name="TOutput">Type of the output</typeparam>
    public interface IFilter<in TInput, out TOutput> : IFilterInput<TInput>
    {
        /// <summary>
        /// Chains another filter behind this filter.
        /// </summary>
        /// <param name="filter">A filter to be chained behind this filter</param>
        public void ChainFilter(IFilterInput<TOutput> filter);
    }
}