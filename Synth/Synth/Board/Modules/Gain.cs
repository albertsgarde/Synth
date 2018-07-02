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

        private Gain(Gain gain)
        {
            this.gain = gain.gain;
            Type = gain.Type;
        }

        public override Module Clone()
        {
            return new Gain(this);
        }

        public override float[] Process(float[] inputs, float frequency)
        {
            return new float[] { inputs[0] * gain };
        }
    }
}
