using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Stuff;
using SynthLib.Data;

namespace SynthLib.Board.Modules
{
    public class InputModule : Module
    {
        public override Connections Inputs { get; }

        public override Connections Outputs { get; }

        public override BoardOutput OutputType => BoardOutput.None;

        private readonly float gain;

        private readonly float[] output;

        public InputModule(int outputs = 0, float gain = 1f)
        {
            Inputs = new ConnectionsArray(1, 1);
            Outputs = new ConnectionsArray(outputs);
            this.gain = gain;
            output = new float[outputs];
        }

        public InputModule(XElement element, SynthData data)
        {
            gain = InvalidModuleSaveElementException.ParseFloat(element.Element("gain"));
            var outputs = InvalidModuleSaveElementException.ParseInt(element.Element("outputs"));
            Inputs = new ConnectionsArray(element.Element("inputs"));
            Outputs = new ConnectionsArray(element.Element("outputs"));
            output = new float[Outputs.Count];
        }

        public override Module Clone(int sampleRate = 44100)
        {
            return new InputModule(output.Length, gain);
        }

        public override Module CreateInstance(XElement element, SynthData data)
        {
            return new InputModule(element, data);
        }

        protected override float[] IntProcess(float[] inputs, long time, bool noteOn, ModuleBoard moduleBoard)
        {
            throw new NotImplementedException();
        }
        
        public override XElement ToXElement(string name)
        {
            var element = base.ToXElement(name);
            element.AddValue("gain", gain);
            return element;
        }
    }
}
