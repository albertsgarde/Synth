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
    public class GlideModifier : Module
    {
        public override Connections Inputs { get; }

        public override Connections Outputs { get; }

        public override BoardOutput OutputType => BoardOutput.GlideTime;

        public GlideModifier()
        {
            Inputs = new ConnectionsArray(1);
            Outputs = new ConnectionsArray(1);
        }

        public override Module Clone(int sampleRate = 44100)
        {
            return new GlideModifier();
        }

        public override Module CreateInstance(XElement element, SynthData data)
        {
            return new GlideModifier();
        }

        protected override float[] IntProcess(float[] inputs, long time, bool noteOn, ModuleBoard moduleBoard)
        {
            return inputs;
        }
    }
}
