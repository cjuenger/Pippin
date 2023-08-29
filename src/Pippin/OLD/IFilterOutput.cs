namespace Pippin.Filters
{
    /// <summary>
    /// Filter part that has no input but allows chaining filters to output to.
    /// It is part of a <see cref="Filter{TInput,TOutput}"/> but can also be used
    /// as the starting point of a pipe.
    /// </summary>
    /// <typeparam name="TOutput">Type of the output</typeparam>
    public interface IFilterOutput<out TOutput>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        public void ChainFilter(IFilterInput<TOutput> filter);
    }
}