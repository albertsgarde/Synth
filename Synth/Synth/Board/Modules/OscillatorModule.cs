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
    public class OscillatorModule : Module
    {
        private readonly IOscillator oscillator;

        private readonly float gain;

        private readonly float frequencyMultiplier;

        public override Connections Inputs { get; }

        public override Connections Outputs { get; }

        public override string Type { get; } = "Oscillator";

        public OscillatorModule(IOscillator oscillator, int outputs, float halfToneOffset = 0, float gain = 1f)
        {
            this.oscillator = oscillator.Clone();
            frequencyMultiplier = (float) Math.Pow(2, (1 / 12d) * halfToneOffset);
            this.gain = gain;
            Inputs = new ConnectionsArray(0);
            Outputs = new ConnectionsArray(outputs);
        }

        private OscillatorModule(OscillatorModule oscMod)
        {
            oscillator = oscMod.oscillator.Clone();
            gain = oscMod.gain;
            frequencyMultiplier = oscMod.frequencyMultiplier;
            Inputs = new ConnectionsArray(0);
            Outputs = new ConnectionsArray(oscMod.Outputs.Count);
            Type = oscMod.Type;
        }

        public override void Reset()
        {
            base.Reset();
            oscillator.Reset();
        }

        public override Module Clone()
        {
            return new OscillatorModule(this);
        }

        public override float[] Process(float[] inputs, float frequency)
        {
            //Console.WriteLine(Outputs[0].Destination);

            oscillator.Next(frequency * frequencyMultiplier);

            var output = new float[Outputs.Count];
            var next = oscillator.CurrentValue();
            for (int i = 0; i < output.Length; ++i)
                output[i] = next * gain;
            return output;
        }
    }
}
