using Stuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Stuff;

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

        public override string Type { get; } = "Mixer";

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

            Type = mixer.Type;
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

            public XElement ToXElement(string name)
            {
                var element = new XElement(name);
                for (int i = 0; i < gains.Length; ++i)
                    element.AddValue("" + i, gains[i]);
                return element;
            }

            public int Length => gains.Length;
        }

        public override Module Clone()
        {
            return new Mixer(this);
        }

        public override float[] Process(float[] inputs, int time, bool noteOn)
        {
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
