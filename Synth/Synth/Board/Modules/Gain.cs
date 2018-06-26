using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Board.Modules
{
    public class Gain : Module
    {
        private float gain;

        public override Connections Inputs { get; }

        public override Connections Outputs { get; }

        public override string Type { get; } = "Gain";

        public Gain(float gain)
        {
            this.gain = gain;
            Inputs = new ConnectionsArray(1);
            Outputs = new ConnectionsArray(1);
        }

        public override void Next()
        {
        }

        public override float[] Process(float[] inputs)
        {
            return new float[] { inputs[0] * gain };
        }
    }
}
