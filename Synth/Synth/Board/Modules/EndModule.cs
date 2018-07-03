using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Board.Modules
{
    public class EndModule : Module
    {
        public override Connections Inputs { get; }

        public override Connections Outputs { get; }

        public override string Type { get; } = "End";

        private float[] output;

        public EndModule()
        {
            Inputs = new FlexConnections();
            Outputs = new ConnectionsArray(0);
            output = new float[1];
        }

        public override Module Clone()
        {
            return new EndModule();
        }

        public override float[] Process(float[] inputs)
        {
            output[0] = 0;
            for (int i = 0; i < inputs.Length; ++i)
                output[0] += inputs[i];
            return output;
        }
    }
}
