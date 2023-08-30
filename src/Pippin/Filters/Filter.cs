using System;
using System.Collections.Generic;
using Pippin.Pipes;

namespace Pippin.Filters
{
    /// <inheritdoc />
    public abstract class Filter<TInput, TOutput> : IFilter<TInput, TOutput>
    {
        /// <summary>
        /// List of connected <see cref="IPipePlug{TInput}"/> plugs
        /// </summary>
        protected readonly List<IPipePlug<TOutput>> PipePlugs = new List<IPipePlug<TOutput>>();

        /// <inheritdoc />
        public void Connect(IPipePlug<TOutput> plug)
        {
            if (plug == null) throw new ArgumentNullException(nameof(plug));
            PipePlugs.Add(plug);
        }
        
        /// <inheritdoc />
        public virtual void Input(TInput input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            var output = Process(input);
            Output(output);
        }

        /// <summary>
        /// Contract for the filtration process
        /// </summary>
        /// <param name="input">Input data</param>
        /// <returns>Returns the filtered/processed output</returns>
        protected abstract TOutput Process(TInput input);
        
        /// <summary>
        /// Output to all connected <see cref="PipePlug{TInput}"/>
        /// </summary>
        /// <param name="output">Output data</param>
        /// <exception cref="ArgumentNullException">The passed argument 'output' is null.</exception>
        protected void Output(TOutput output)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));
            foreach (var pipePlug in PipePlugs) pipePlug.Input(output);
        }
    }
}