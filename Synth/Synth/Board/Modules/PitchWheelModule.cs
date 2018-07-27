using SynthLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SynthLib.Board.Modules
{
    public class PitchWheelModule : Module
    {
        public override Connections Inputs { get; }

        public override Connections Outputs { get; }

        public override BoardOutput OutputType => BoardOutput.None;

        private readonly float[] output;

        public const float TWO_OVER_16384 = 2f / 16384;

        public PitchWheelModule()
        {
            output = new float[1];
            Inputs = new ConnectionsArray(0);
            Outputs = new ConnectionsArray(1);
        }

        public override Module Clone(int sampleRate = 44100)
        {
            return new PitchWheelModule();
        }

        public override Module CreateInstance(XElement element, SynthData data)
        {
            return new PitchWheelModule();
        }

        protected override float[] IntProcess(float[] inputs, long time, bool noteOn, ModuleBoard moduleBoard)
        {
            output[0] = moduleBoard.PitchWheel * TWO_OVER_16384 - 1;
            return output;
        }
    }
}
