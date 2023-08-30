using System;
using System.Collections.Generic;

namespace Pippin.Pipes
{
    /// <inheritdoc />
    public abstract class PipeSocket<TOutput> : IPipeSocket<TOutput>
    {
        private readonly List<IPipePlug<TOutput>> _pipePlugs = new List<IPipePlug<TOutput>>();

        /// <inheritdoc />
        public void Connect(IPipePlug<TOutput> plug)
        {
            if (plug == null) throw new ArgumentNullException(nameof(plug));
            _pipePlugs.Add(plug);
        }
        
        /// <summary>
        /// Output to all chained filters
        /// </summary>
        /// <param name="output">Output data</param>
        /// <exception cref="ArgumentNullException">The passed argument 'output' is null.</exception>
        protected virtual void Output(TOutput output)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));
            foreach (var pipePlug in _pipePlugs) pipePlug.Input(output);
        }
    }
}