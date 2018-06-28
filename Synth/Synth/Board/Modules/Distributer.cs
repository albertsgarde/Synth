using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public float[] InputWeights { get; }

        private float totalInputWeight;

        public float[] OutputWeights { get; }

        private float totalOutputWeight;

        public override string Type { get; } = "Mixer";

        public Distributer(int inputs, int outputs)
        {
            Inputs = new ConnectionsArray(inputs);
            Outputs = new ConnectionsArray(outputs);

            InputWeights = new float[inputs];
            for (int i = 0; i < inputs; ++i)
            {
                InputWeights[i] = 1;
            }
            totalInputWeight = InputWeights.Sum();

            OutputWeights = new float[outputs];
            for (int i = 0; i < outputs; ++i)
            {
                OutputWeights[i] = 1;
            }
            totalOutputWeight = OutputWeights.Sum();
        }

        public Distributer(float[] inputWeights, float[] outputWeights)
        {
            Inputs = new ConnectionsArray(inputWeights.Length);
            Outputs = new ConnectionsArray(outputWeights.Length);

            InputWeights = new float[inputWeights.Length];
            inputWeights.CopyTo(InputWeights, 0);
            totalInputWeight = InputWeights.Sum();

            OutputWeights = new float[outputWeights.Length];
            outputWeights.CopyTo(OutputWeights, 0);
            totalOutputWeight = OutputWeights.Sum();
        }

        public override float[] Process(float[] inputs)
        {
            var totalInput = 0f;
            for (int i = 0; i < inputs.Length; ++i)
                totalInput += inputs[i] * InputWeights[i];
            totalInput /= (totalInputWeight/inputs.Length);

            var totalOutput = totalInput / totalOutputWeight;
            var result = new float[Outputs.Count];
            for (int i = 0; i < result.Length; ++i)
                result[i] = totalOutput * OutputWeights[i];

            return result;
        }
    }
}
