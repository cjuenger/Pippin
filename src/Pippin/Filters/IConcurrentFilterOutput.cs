using System;

namespace Pippin.Filters
{
    /// <summary>
    /// Concurrent filter part that has no input but allows chaining filters to output to.
    /// It is part of a <see cref="ConcurrentFilter{TInput,TOutput}"/> but can also be used
    /// as the starting point of a pipe.
    /// </summary>
    /// <typeparam name="TOutput">Type of the output</typeparam>
    public interface IConcurrentFilterOutput<out TOutput> : IFilterOutput<TOutput>, IDisposable {}
}