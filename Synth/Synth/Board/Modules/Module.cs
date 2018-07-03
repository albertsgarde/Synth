using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Board.Modules
{
    public abstract class Module
    {
        public abstract Connections Inputs { get; }

        public abstract Connections Outputs { get; }

        public abstract string Type { get; }

        public string Name { get; set; }

        public abstract float[] Process(float[] inputs);

        public int num;

        public virtual void UpdateFrequency(float frequency)
        {
            
        }

        public IEnumerable<Connection> Connections()
        {
            return Inputs.Concat(Outputs);
        }

        public virtual void Reset()
        {

        }

        /// <summary>
        /// The clone should be complete apart from the connections, which should be empty.
        /// </summary>
        /// <returns>A clone of the called module.</returns>
        public abstract Module Clone();
    }
}
