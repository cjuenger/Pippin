namespace Pippin.Pipes
{
    /// <summary>
    /// A <see cref="IPipeSocket{TOutput}"/> that provides output for connected <see cref="IPipePlug{TInput}"/>
    /// </summary>
    /// <typeparam name="TOutput">Output for the <see cref="PipePlug{TInput}"/></typeparam>
    public interface IPipeSocket<out TOutput>
    {
        /// <summary>
        /// Connects a plug with this socket.
        /// </summary>
        /// <param name="plug">A plug to be connected to this socket</param>
        void Connect(IPipePlug<TOutput> plug);
    }
}