namespace Pippin.Filters
{
    public interface IFilterInput<in TInput>
    {
        /// <summary>
        ///     Invokes the filter operation.
        /// </summary>
        /// <param name="input">The pipeline operation's operand.</param>
        /// <returns>Returns the task of the operation.</returns>
        /// <remarks>
        ///     Not every operation is respectively need to be async but for a narrow and unambiguous interface it has
        ///     been decided to use an async method call at this point.
        /// </remarks>
        void Input(TInput input);
    }
}