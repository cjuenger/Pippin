using Pippin.Filters;

namespace Pippin.Pipes
{
    public abstract class PipeEnd<TInput> : IFilterInput<TInput>
    {
        public abstract void Input(TInput input);
    }
}