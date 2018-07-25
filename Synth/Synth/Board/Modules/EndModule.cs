using Stuff;
using SynthLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SynthLib.Board.Modules
{
    public class EndModule : Module
    {
        public override Connections Inputs { get; }

        public override Connections Outputs { get; }

        public override ModuleType Type { get; }

        private readonly float[] output;

        public EndModule()
        {
            useable = false;
        }

        public EndModule(bool right)
        {
            Type = right ? ModuleType.RightOut : ModuleType.LeftOut;
            Inputs = new FlexConnections();
            Outputs = new ConnectionsArray(1);
            output = new float[1];
        }

        public EndModule(EndModule endModule)
        {
            Type = endModule.Type;
            Inputs = new FlexConnections(endModule.Inputs.Count);
            Outputs = new ConnectionsArray(1);
            output = new float[1];
        }

        public EndModule(XElement element)
        {
            Type = InvalidModuleSaveElementException.ParseInt(element.Element("out")) == 1 ? ModuleType.RightOut : ModuleType.LeftOut;
            Inputs = new FlexConnections(element.Element("inputs"));
            Outputs = new ConnectionsArray(element.Element("outputs"));
            output = new float[1];
        }

        public override Module Clone(int sampleRate = 44100)
        {
            return new EndModule(this);
        }

        public override Module CreateInstance(XElement element, SynthData data)
        {
            return new EndModule(element);
        }

        protected override float[] IntProcess(float[] inputs, long time, bool noteOn)
        {
            output[0] = 0;
            for (int i = 0; i < inputs.Length; ++i)
                output[0] += inputs[i];
            return output;
        }

        public override XElement ToXElement(string name)
        {
            var element = base.ToXElement(name);
            element.AddValue("out", Type == ModuleType.RightOut ? 1 : 0);
            return element;
        }
    }
}
