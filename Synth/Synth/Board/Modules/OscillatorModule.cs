using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynthLib.Oscillators;
using SynthLib.Music;
using Stuff;
using System.Xml.Linq;

namespace SynthLib.Board.Modules
{
    public class OscillatorModule : Module
    {
        private readonly IOscillator oscillator;

        private readonly float gain;

        private readonly float frequencyMultiplier;

        public override Connections Inputs { get; } // 0: Gain modifier;

        public override Connections Outputs { get; }

        public override string Type { get; } = "Oscillator";

        private float[] output;

        public OscillatorModule(IOscillator oscillator, int outputs, float halfToneOffset = 0, float gain = 1f)
        {
            this.oscillator = oscillator.Clone();
            frequencyMultiplier = (float) Math.Pow(2, (1 / 12d) * halfToneOffset);
            this.gain = gain;
            Inputs = new ConnectionsArray(1, 1);
            Outputs = new ConnectionsArray(outputs);
            output = new float[outputs];
        }

        private OscillatorModule(OscillatorModule oscMod)
        {
            oscillator = oscMod.oscillator.Clone();
            gain = oscMod.gain;
            frequencyMultiplier = oscMod.frequencyMultiplier;
            Inputs = new ConnectionsArray(1, 1);
            Outputs = new ConnectionsArray(oscMod.Outputs.Count);
            output = new float[Outputs.Count];
            Type = oscMod.Type;
        }

        public override void UpdateFrequency(float frequency)
        {
            oscillator.Frequency = frequency * frequencyMultiplier;
        }

        public override Module Clone()
        {
            return new OscillatorModule(this);
        }

        public override float[] Process(float[] inputs, int time, bool noteOn)
        {
            var next = oscillator.NextValue() * gain * (inputs[0] + 1);
            for (int i = 0; i < output.Length; ++i)
                output[i] = next;
            return output;
        }

        public override XElement ToXElement(string name)
        {
            var element = base.ToXElement(name);
            element.Add(oscillator.ToXElement("osc"));
            element.AddValue("gain", gain);
            element.AddValue("frequencyMultiplier", frequencyMultiplier);
            return element;
        }
    }
}
