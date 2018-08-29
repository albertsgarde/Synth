using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SynthLib.Data;

namespace SynthLib.Board.Modules
{
    public class Multiply : Module
    {
        public override Connections Inputs { get; }

        public override Connections Outputs { get; }

        public override BoardOutput OutputType => BoardOutput.None;

        private readonly float[] output;

        public Multiply()
        {
            output = new float[1];

            Inputs = new ConnectionsArray(2);
            Outputs = new ConnectionsArray(1);
        }

        public override Module Clone(int sampleRate = 44100)
        {
            return new Multiply();
        }

        public override Module CreateInstance(XElement element, SynthData data)
        {
            return new Multiply();
        }

        protected override float[] IntProcess(float[] inputs, long time, bool noteOn, ModuleBoard moduleBoard)
        {
            output[0] = inputs[0] * inputs[1];
            return output;
        }
    }
}
