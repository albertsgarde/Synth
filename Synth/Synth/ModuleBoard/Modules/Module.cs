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

        public abstract float[] Process(float[] inputs);

        public string Name { get; set; }

        public IEnumerable<Connection> Connections()
        {
            return Inputs.Concat(Outputs);
        }
    }
}
