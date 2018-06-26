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

        public EndModule()
        {
            Inputs = new FlexConnections();
            Outputs = new ConnectionsArray(0);
        }

        public override void Next()
        {
        }

        public override float[] Process(float[] inputs)
        {
            return new float[] { inputs.Sum() };
        }
    }
}
