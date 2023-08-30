namespace Pippin.Pipes
{
    /// <summary>
    /// A <see cref="IPipePlug{TInput}"/> that receives input from a <see cref="IPipeSocket{TOutput}"/>
    /// </summary>
    /// <typeparam name="TInput">Input for the <see cref="PipePlug{TInput}"/></typeparam>
    public interface IPipePlug<in TInput>
    {
        /// <summary>
        /// Provides input to the <see cref="IPipePlug{TInput}"/>
        /// </summary>
        /// <param name="input">Input of the <see cref="IPipePlug{TInput}"/></param>
        void Input(TInput input);
    }
}