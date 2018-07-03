using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff;

namespace SynthLib.Board.Modules
{
    /// <summary>
    /// Distributes incoming signals according to the specified input and output weights.
    /// The total output amplitude is equal to the weighted total input amplitude.
    /// </summary>
    public class Distributer : Module
    {
        public override Connections Inputs { get; }

        public override Connections Outputs { get; }

        public Weights InputWeights { get; }

        public Weights OutputWeights { get; }

        public override string Type { get; } = "Mixer";

        public Distributer(int inputs, int outputs)
        {
            Inputs = new ConnectionsArray(inputs);
            Outputs = new ConnectionsArray(outputs);
            
            InputWeights = new Weights(ContainerUtils.UniformArray(1f, inputs));

            OutputWeights = new Weights(ContainerUtils.UniformArray(1f, outputs));
        }

        public Distributer(float[] inputWeights, float[] outputWeights)
        {
            Inputs = new ConnectionsArray(inputWeights.Length);
            Outputs = new ConnectionsArray(outputWeights.Length);

            InputWeights = new Weights(inputWeights);
            OutputWeights = new Weights(outputWeights);
        }

        private Distributer(Distributer distributer)
        {
            Inputs = new ConnectionsArray(distributer.Inputs.Count);
            Outputs = new ConnectionsArray(distributer.Outputs.Count);
            InputWeights = new Weights(distributer.InputWeights);
            OutputWeights = new Weights(distributer.OutputWeights);
        }

        public struct Weights
        {
            public readonly float[] weights;

            public float total;

            public Weights(float[] weights)
            {
                this.weights = new float[weights.Length];
                weights.CopyTo(this.weights, 0);
                total = weights.Sum();
            }

            public Weights(Weights weights)
            {
                this.weights = new float[weights.weights.Length];
                weights.weights.CopyTo(this.weights, 0);
                total = weights.total;
            }
        }

        public override Module Clone()
        {
            return new Distributer(this);
        }

        public override float[] Process(float[] inputs)
        {
            var totalInput = 0f;
            for (int i = 0; i < inputs.Length; ++i)
                totalInput += inputs[i] * InputWeights.weights[i];
            totalInput /= (InputWeights.total/inputs.Length);

            var totalOutput = totalInput / OutputWeights.total;
            var result = new float[Outputs.Count];
            for (int i = 0; i < result.Length; ++i)
                result[i] = totalOutput * OutputWeights.weights[i];
            return result;
        }
    }
}
