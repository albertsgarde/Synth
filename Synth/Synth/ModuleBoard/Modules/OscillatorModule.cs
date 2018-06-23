using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynthLib.Oscillators;

namespace SynthLib.Board.Modules
{
    public class OscillatorModule : Module
    {
        private IOscillator oscillator;

        public override Connections Inputs { get; }

        public override Connections Outputs { get; }

        public override string Type { get; } = "Oscillator";

        public OscillatorModule(IOscillator oscillator, int outputs)
        {
            this.oscillator = oscillator;
            Inputs = new ConnectionsArray(0);
            Outputs = new ConnectionsArray(outputs);
        }

        public override float[] Process(float[] inputs)
        {
            var next = oscillator.NextValue(440);
            var output = new float[Outputs.Count];
            for (int i = 0; i < output.Length; ++i)
                output[i] = next;
            return output;
        }
    }
}
