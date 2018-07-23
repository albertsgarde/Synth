using Stuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Diagnostics;
using SynthLib.Data;

namespace SynthLib.Board.Modules
{
    /// <summary>
    /// Applies the specified gain to each input and outputs the total to every output adjusted for output gain.
    /// </summary>
    public class Mixer : Module
    {
        public override Connections Inputs { get; }

        public override Connections Outputs { get; }

        public Gains InputGains { get; }

        public Gains OutputGains { get; }

        private const string TYPE = "Mixer";

        public override string Type { get; } = TYPE;

        public Mixer()
        {
            useable = false;
            Inputs = new ConnectionsArray(0);
            Outputs = new ConnectionsArray(0);
            InputGains = new Gains(0);
            OutputGains = new Gains(0);
        }

        public Mixer(int inputs, int outputs)
        {
            Inputs = new ConnectionsArray(inputs);
            Outputs = new ConnectionsArray(outputs);
            InputGains = new Gains(inputs);
            OutputGains = new Gains(outputs);
        }

        public Mixer(float[] inputGains, float[] outputGains)
        {
            Inputs = new ConnectionsArray(inputGains.Length);
            Outputs = new ConnectionsArray(outputGains.Length);
            InputGains = new Gains(inputGains);
            OutputGains = new Gains(outputGains);
        }

        private Mixer(Mixer mixer)
        {
            Inputs = new ConnectionsArray(mixer.Inputs.Count);
            Outputs = new ConnectionsArray(mixer.Outputs.Count);
            
            InputGains = mixer.InputGains;
            OutputGains = mixer.OutputGains;
        }

        private Mixer(XElement element)
        {
            Inputs = new ConnectionsArray(element.Element("inputs"));
            Outputs = new ConnectionsArray(element.Element("outputs"));
            InputGains = new Gains(element.Element("inputGains"));
            OutputGains = new Gains(element.Element("outputGains"));
        }

        public struct Gains : ISaveable
        {
            private readonly float[] gains;

            public float this[int i] => gains[i];

            public Gains(int inputs)
            {
                gains = new float[inputs];
                for (int i = 0; i < inputs; ++i)
                {
                    gains[i] = 1;
                }
            }

            public Gains(float[] gains)
            {
                this.gains = new float[gains.Length];
                gains.CopyTo(this.gains, 0);
            }

            public Gains(XElement element)
            {
                gains = new float[element.Elements().Count()];
                for (int i = 0; i < gains.Length; ++i)
                {
                    if (!float.TryParse(element.ElementValue("_" + i), out gains[i]))
                        throw new InvalidModuleSaveElementException(element);
                }
            }

            public XElement ToXElement(string name)
            {
                var element = new XElement(name);
                for (int i = 0; i < gains.Length; ++i)
                    element.AddValue("_" + i, gains[i]);
                return element;
            }

            public int Length => gains.Length;
        }

        public override Module Clone(int sampleRate = 44100)
        {
            return new Mixer(this);
        }

        public override Module CreateInstance(XElement element, SynthData data)
        {
            return new Mixer(element);
        }

        protected override float[] IntProcess(float[] inputs, long time, bool noteOn)
        {
            Debug.Assert(useable);
            var totalInput = 0f;
            for (int i = 0; i < inputs.Length; ++i)
                totalInput += inputs[i] * InputGains[i];

            var result = new float[Outputs.Count];
            for (int i = 0; i < result.Length; ++i)
                result[i] = totalInput * OutputGains[i];
            
            return result;
        }

        public override XElement ToXElement(string name)
        {
            var element = base.ToXElement(name);
            element.Add(InputGains.ToXElement("inputGains"));
            element.Add(OutputGains.ToXElement("outputGains"));
            return element;
        }
    }
}
