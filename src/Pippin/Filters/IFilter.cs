using Pippin.Pipes;

namespace Pippin.Filters
{
    /// <summary>
    /// A filter that has a <see cref="PipePlug{TInput}"/> for input and <see cref="PipeSocket{TOutput}"/> for output.
    /// </summary>
    /// <typeparam name="TInput">Type of the input</typeparam>
    /// <typeparam name="TOutput">Type of the output</typeparam>
    public interface IFilter<in TInput, out TOutput> : IPipeSocket<TOutput>, IPipePlug<TInput>
    {
    }
}