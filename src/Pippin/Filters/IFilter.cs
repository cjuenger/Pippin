namespace Pippin.Filters
{
    /// <summary>
    /// A filter that has input and output.
    /// </summary>
    /// <typeparam name="TInput">Type of the input</typeparam>
    /// <typeparam name="TOutput">Type of the output</typeparam>
    public interface IFilter<in TInput, out TOutput> : IFilterInput<TInput>, IFilterOutput<TOutput> {}
}