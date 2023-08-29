namespace Pippin.Filters
{
    /// <summary>
    /// Filter part that has no output but only input.
    /// It is part of a <see cref="Filter{TInput,TOutput}"/> 
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    public interface IFilterInput<in TInput>
    {
        
        /// <summary>
        /// Inputs the input item into the filter.
        /// </summary>
        /// <param name="input">Input item</param>
        void Input(TInput input);
    }
}