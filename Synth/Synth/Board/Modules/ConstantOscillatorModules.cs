using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynthLib.Oscillators;
using SynthLib.Music;

using Stuff;
using System.Xml.Linq;
using SynthLib.Data;

namespace SynthLib.Board.Modules
{
    public class ConstantOscillatorModule : Module
    {

        private readonly Oscillator oscillator;

        private readonly float gain;

        private readonly float frequency;

        public override Connections Inputs { get; } // 0: Gain modifier;

        public override Connections Outputs { get; }

        public override BoardOutput OutputType { get; } = BoardOutput.None;

        private float[] output;

        public ConstantOscillatorModule()
        {
            useable = false;
        }

        public ConstantOscillatorModule(Oscillator oscillator, int outputs, float frequency, float gain = 1f, int sampleRate = 44100)
        {
            this.oscillator = oscillator.Clone(sampleRate);
            this.oscillator.Frequency = frequency;
            this.gain = gain;
            this.frequency = frequency;
            Inputs = new ConnectionsArray(1, 1);
            Outputs = new ConnectionsArray(outputs);
            output = new float[outputs];
        }

        private ConstantOscillatorModule(ConstantOscillatorModule oscMod, int sampleRate = 44100)
        {
            oscillator = oscMod.oscillator.Clone(sampleRate);
            gain = oscMod.gain;
            frequency = oscMod.frequency;
            oscillator.Frequency = frequency;
            Inputs = new ConnectionsArray(1, 1);
            Outputs = new ConnectionsArray(oscMod.Outputs.Count);
            output = new float[Outputs.Count];
        }

        private ConstantOscillatorModule(XElement element, SynthData data)
        {
            oscillator = data.OscillatorTypes[element.Element("osc").ElementValue("type")].Instance.CreateInstance(element.Element("osc"), data);
            gain = InvalidModuleSaveElementException.ParseFloat(element.Element("gain"));
            frequency = InvalidModuleSaveElementException.ParseFloat(element.Element("frequency"));
            oscillator.Frequency = frequency;
            Inputs = new ConnectionsArray(element.Element("inputs"));
            Outputs = new ConnectionsArray(element.Element("outputs"));
            output = new float[Outputs.Count];
        }

        public override Module Clone(int sampleRate = 44100)
        {
            return new ConstantOscillatorModule(this, sampleRate);
        }

        public override Module CreateInstance(XElement element, SynthData data)
        {
            return new ConstantOscillatorModule(element, data);
        }

        protected override float[] IntProcess(float[] inputs, long time, bool noteOn, ModuleBoard moduleBoard)
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
            element.AddValue("frequency", frequency);
            return element;
        }
    }
}
