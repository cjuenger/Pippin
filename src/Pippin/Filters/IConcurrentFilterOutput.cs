using System;

namespace Pippin.Filters
{
    public interface IConcurrentFilterOutput<TOutput> : IFilterOutput<TOutput>, IDisposable {}
}