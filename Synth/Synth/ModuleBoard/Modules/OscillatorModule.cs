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

        private float gain;

        private float frequencyMultiplier;

        public override Connections Inputs { get; }

        public override Connections Outputs { get; }

        public override string Type { get; } = "Oscillator";

        public OscillatorModule(IOscillator oscillator, int outputs, float gain = 1, float halfToneOffset = 0)
        {
            this.oscillator = oscillator.Clone();
            frequencyMultiplier = (float) Math.Pow(2, (1 / 12d) * halfToneOffset);
            this.gain = gain;
            Inputs = new ConnectionsArray(0);
            Outputs = new ConnectionsArray(outputs);
        }

        public override float[] Process(float[] inputs)
        {
            var frequency = 440d;
            var next = oscillator.NextValue(frequency * frequencyMultiplier);
            var output = new float[Outputs.Count];
            for (int i = 0; i < output.Length; ++i)
                output[i] = next;
            return output;
        }
    }
}
