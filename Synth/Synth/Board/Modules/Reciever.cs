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
    public class Reciever : Module
    {
        public override Connections Inputs { get; }

        public override Connections Outputs { get; }

        public override BoardOutput OutputType => BoardOutput.None;

        private readonly int id;

        private readonly float[] output;

        public Reciever()
        {
            useable = false;
        }

        public Reciever(int id)
        {
            this.id = id;
            Inputs = new ConnectionsArray(0);
            Outputs = new ConnectionsArray(1);
            output = new float[1];
        }

        public Reciever(XElement element) : this(InvalidModuleSaveElementException.ParseInt(element.Element("id")))
        {

        }

        public override Module Clone(int sampleRate = 44100)
        {
            return new Reciever(id);
        }

        public override Module CreateInstance(XElement element, SynthData data)
        {
            return new Reciever(element);
        }

        protected override float[] IntProcess(float[] inputs, long time, bool noteOn, ModuleBoard moduleBoard)
        {
            output[0] = moduleBoard.RecieveValue(id);
            return output;
        }

        public override XElement ToXElement(string name)
        {
            var element = base.ToXElement(name);
            element.AddValue("id", id);
            return element;
        }
    }
}
