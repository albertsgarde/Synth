using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Effects
{
    public class Boost : IEffect
    {
        public double Gain { get; }

        public Boost(double gain, bool decibel = false)
        {
            Gain = decibel ? NAudio.Utils.Decibels.DecibelsToLinear(gain) : gain;
        }

        public IEffect Clone()
        {
            return new Boost(Gain);
        }

        public float Next(float input)
        {
            return input * (float)Gain;
        }
    }
}
