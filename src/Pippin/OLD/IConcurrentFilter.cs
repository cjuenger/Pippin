namespace Pippin.Filters
{
    /// <summary>
    /// A thread-safe concurrent filter that has input and output.
    /// </summary>
    /// <typeparam name="TInput">Type of the input</typeparam>
    /// <typeparam name="TOutput">Type of the output</typeparam>
    public interface IConcurrentFilter<in TInput, out TOutput> : IFilterInput<TInput>, IConcurrentFilterOutput<TOutput> {}
}