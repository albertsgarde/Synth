using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Board.Modules
{
    /// <summary>
    /// Flips the incoming signal. *-1
    /// </summary>
    public class Flip : Module
    {
        public override Connections Inputs { get; }

        public override Connections Outputs { get; }

        public override string Type { get; } = "Flip";

        public Flip()
        {
            Inputs = new ConnectionsArray(1);
            Outputs = new ConnectionsArray(1);
        }

        public override Module Clone()
        {
            return new Flip();
        }

        public override float[] Process(float[] inputs)
        {
            return new float[] { inputs[0] * -1 };
        }
    }
}
