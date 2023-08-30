namespace Pippin.Pipes
{
    /// <inheritdoc />
    public abstract class PipePlug<TInput> : IPipePlug<TInput>
    {
        /// <inheritdoc />
        public virtual void Input(TInput input)
        {
            ProcessInput(input);
        }

        /// <summary>
        /// Contract for the actual processing of the <see cref="PipePlug{TInput}"/>
        /// </summary>
        /// <param name="input">Input of the <see cref="PipePlug{TInput}"/></param>
        protected abstract void ProcessInput(TInput input);
    }
}