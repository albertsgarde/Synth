using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Board.Modules
{
    /// <summary>
    /// Delays the signal.
    /// </summary>
    public class Delay : Module
    {
        private Queue<float> prevs;

        public override Connections Inputs { get; }

        public override Connections Outputs { get; }

        public override string Type { get; } = "Delay";
        
        /// <param name="delaySamples">How many samples to delay the signal by.</param>
        public Delay(int delaySamples)
        {
            prevs = new Queue<float>(delaySamples + 1);
            for (int i = 0; i < delaySamples; ++i)
                prevs.Enqueue(0);
            Inputs = new ConnectionsArray(1);
            Outputs = new ConnectionsArray(1);
        }

        public override float[] Process(float[] inputs)
        {
            prevs.Enqueue(inputs[0]);
            return new float[] { prevs.Dequeue() };
        }
    }
}
