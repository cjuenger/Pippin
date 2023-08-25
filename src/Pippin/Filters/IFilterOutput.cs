namespace Pippin.Filters
{
    /// <summary>
    /// Filter part that has no input but allows chaining filters to output to.
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