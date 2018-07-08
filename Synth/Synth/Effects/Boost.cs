using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Effects
{
    public class Boost : IEffect
    {
        public int Values => 1;

        public float Gain { get; }

        public Boost(float gain, bool decibel = false)
        {
            Gain = decibel ? (float)NAudio.Utils.Decibels.DecibelsToLinear(gain) : gain;
        }

        public IEffect Clone()
        {
            return new Boost(Gain);
        }

        public float Next(float[] input)
        {
            return input[Values] * Gain * (input[0] + 1);
        }
    }
}
