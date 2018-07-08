using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynthLib.Oscillators;
using SynthLib.Music;
using SynthLib.ValueProviders;
using Stuff;

namespace SynthLib.Board.Modules
{
    public class ConstantOscillatorModule : Module
    {
        private readonly IOscillator oscillator;

        private readonly float gain;

        public override Connections Inputs { get; } // 0: Gain modifier;

        public override Connections Outputs { get; }

        public override string Type { get; } = "ConstOscillator";

        private float[] output;

        public ConstantOscillatorModule(IOscillator oscillator, int outputs, float frequency, float gain = 1f)
        {
            this.oscillator = oscillator.Clone();
            oscillator.Frequency = frequency;
            this.gain = gain;
            Inputs = new ConnectionsArray(1, 1);
            Outputs = new ConnectionsArray(outputs);
            output = new float[outputs];
        }

        private ConstantOscillatorModule(ConstantOscillatorModule oscMod)
        {
            oscillator = oscMod.oscillator.Clone();
            gain = oscMod.gain;
            Inputs = new ConnectionsArray(1, 1);
            Outputs = new ConnectionsArray(oscMod.Outputs.Count);
            output = new float[Outputs.Count];
            Type = oscMod.Type;
        }

        public override Module Clone()
        {
            return new ConstantOscillatorModule(this);
        }

        public override float[] Process(float[] inputs, int time, bool noteOn)
        {
            var next = oscillator.NextValue() * gain;
            for (int i = 0; i < output.Length; ++i)
                output[i] = next;
            return output;
        }
    }
}
