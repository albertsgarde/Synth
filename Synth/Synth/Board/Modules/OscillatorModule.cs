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
    public class OscillatorModule : Module
    {
        private readonly Oscillator oscillator;

        private readonly float gain;

        private readonly float frequencyMultiplier;

        public override Connections Inputs { get; } // 0: Gain modifier;

        public override Connections Outputs { get; }

        public override BoardOutput OutputType { get; } = BoardOutput.None;

        private float[] output;

        public OscillatorModule()
        {
            useable = false;
        }

        public OscillatorModule(Oscillator oscillator, int outputs, float halfToneOffset = 0, float gain = 1f, int sampleRate = 44100)
        {
            this.oscillator = oscillator.Clone(sampleRate);
            frequencyMultiplier = (float) Tone.FrequencyMultiplierFromNoteOffset(halfToneOffset);
            this.gain = gain;
            Inputs = new ConnectionsArray(1, 1);
            Outputs = new ConnectionsArray(outputs);
            output = new float[outputs];
        }

        private OscillatorModule(OscillatorModule oscMod, int sampleRate)
        {
            oscillator = oscMod.oscillator.Clone(sampleRate);
            gain = oscMod.gain;
            frequencyMultiplier = oscMod.frequencyMultiplier;
            Inputs = new ConnectionsArray(1, 1);
            Outputs = new ConnectionsArray(oscMod.Outputs.Count);
            output = new float[Outputs.Count];
        }

        private OscillatorModule(XElement element, SynthData data)
        {
            oscillator = data.OscillatorTypes[element.Element("osc").ElementValue("type")].Instance.CreateInstance(element.Element("osc"), data);
            gain = InvalidModuleSaveElementException.ParseFloat(element.Element("gain"));
            frequencyMultiplier = InvalidModuleSaveElementException.ParseFloat(element.Element("frequencyMultiplier"));
            var outputs = InvalidModuleSaveElementException.ParseInt(element.Element("outputs"));
            Inputs = new ConnectionsArray(element.Element("inputs"));
            Outputs = new ConnectionsArray(element.Element("outputs"));
            output = new float[Outputs.Count];
        }

        public override void UpdateFrequency(float frequency)
        {
            oscillator.Frequency = frequency * frequencyMultiplier;
        }

        public override Module Clone(int sampleRate = 44100) => new OscillatorModule(this, sampleRate);

        public override Module CreateInstance(XElement element, SynthData data)
        {
            return new OscillatorModule(element, data);
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
            element.AddValue("frequencyMultiplier", frequencyMultiplier);
            return element;
        }
    }
}
