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
    public class Transmitter : Module
    {
        public override Connections Inputs { get; }

        public override Connections Outputs { get; }

        public override BoardOutput OutputType => BoardOutput.None;

        private readonly int id;

        private readonly float[] output;

        private readonly bool throughPut;

        public Transmitter()
        {
            useable = false;
        }

        public Transmitter(int id, bool throughPut = false)
        {
            this.id = id;
            this.throughPut = throughPut;
            Inputs = new ConnectionsArray(1);
            Outputs = new ConnectionsArray(throughPut ? 1 : 0);
            output = new float[0];
        }

        public Transmitter(XElement element) : this(InvalidModuleSaveElementException.ParseInt(element.Element("id")), InvalidModuleSaveElementException.ParseBool(element.Element("throughPut")))
        {

        }

        public override Module Clone(int sampleRate = 44100)
        {
            return new Transmitter(id, throughPut);
        }

        public override Module CreateInstance(XElement element, SynthData data)
        {
            return new Transmitter(element);
        }

        protected override float[] IntProcess(float[] inputs, long time, bool noteOn, ModuleBoard moduleBoard)
        {
            moduleBoard.TransmitValue(id, inputs[0]);
            if (throughPut)
                return inputs;
            else
                return output;
        }

        public override XElement ToXElement(string name)
        {
            var element = base.ToXElement(name);
            element.AddValue("id", id);
            element.AddValue("throughPut", throughPut);
            return element;
        }
    }
}
