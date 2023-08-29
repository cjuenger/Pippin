using Pippin.Filters;

namespace Pippin.Pipes
{
    public abstract class PipeStart<TOutput>
    {
        /// <summary>
        /// Chains a filter behind this start of the pipe.
        /// </summary>
        /// <param name="filter">A filter to be chained behind this filter</param>
        public abstract void ChainFilter(IFilterInput<TOutput> filter);
    }
}