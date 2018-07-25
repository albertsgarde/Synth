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
    public class Pan : Module
    {
        public override Connections Inputs { get; } // 0: Pan

        public override Connections Outputs { get; }

        public override BoardOutput OutputType => BoardOutput.None;

        /// <summary>
        /// A value between -1 and 1, where -1 all signal goes to the lowest output (highest index), and 1 means all signal goes to the highest output (lowest index).
        /// </summary>
        private readonly float pan;

        private readonly float[] output;

        public Pan() : this(0)
        {
            
        }

        public Pan(float pan)
        {
            Inputs = new ConnectionsArray(2, 1);
            Outputs = new ConnectionsArray(2);
            output = new float[2];
            this.pan = pan;
        }

        private Pan(XElement element) : this(InvalidModuleSaveElementException.ParseFloat(element.Element("pan")))
        {
        }

        protected override float[] IntProcess(float[] inputs, long time, bool noteOn, ModuleBoard moduleBoard)
        {
            var curPan = pan + (inputs[0]);
            if (curPan < -1 || curPan > 1)
                throw new InvalidModuleValue(this, curPan, $"Pan must be between -1 and 1. Module pan value: {pan} Pan input: {inputs[0]}");
            output[0] = ValueFromPan(inputs[1], curPan);
            output[1] = ValueFromPan(inputs[1], -curPan);
            return output;
        }

        private float ValueFromPan(float value, float pan) => value * (pan + 1);

        public override Module Clone(int sampleRate = 44100)
        {
            return new Pan(pan);
        }

        public override Module CreateInstance(XElement element, SynthData data)
        {
            return new Pan(element);
        }

        public override XElement ToXElement(string name)
        {
            var element = base.ToXElement(name);
            element.AddValue("pan", pan);
            return element;
        }
    }
}
