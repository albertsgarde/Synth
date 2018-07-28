using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SynthLib.Data;
using Stuff;

namespace SynthLib.Board.Modules
{
    public class BoardModifierModule : Module
    {
        public override Connections Inputs { get; }

        public override Connections Outputs { get; }

        public override BoardOutput OutputType { get; }

        public BoardModifierModule()
        {
            useable = false;
        }

        public BoardModifierModule(BoardOutput output)
        {
            Inputs = new ConnectionsArray(1);
            Outputs = new ConnectionsArray(1);
            OutputType = output;
        }

        public BoardModifierModule(XElement element) : this((BoardOutput)InvalidModuleSaveElementException.ParseInt(element.Element("output")))
        {

        }

        public override Module Clone(int sampleRate = 44100)
        {
            return new BoardModifierModule(OutputType);
        }

        public override Module CreateInstance(XElement element, SynthData data)
        {
            return new BoardModifierModule(element);
        }

        protected override float[] IntProcess(float[] inputs, long time, bool noteOn, ModuleBoard moduleBoard)
        {
            return inputs;
        }

        public override XElement ToXElement(string name)
        {
            var element = base.ToXElement(name);
            element.AddValue("output", (int)OutputType);
            return element;
        }
    }
}
