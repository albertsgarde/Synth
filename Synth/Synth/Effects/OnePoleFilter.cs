using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Effects
{
    public class OnePoleFilter : IEffect
    {
        public int Values => 1;

        private float prev;

        private readonly float a1;

        private const float MAX_A1 = 0.999f;

        public OnePoleFilter(float a1)
        {
            prev = 0;
            this.a1 = a1;
        }

        public IEffect Clone()
        {
            return new OnePoleFilter(a1);
        }

        public float Next(float[] input)
        {
            prev = input[Values] + prev * -a1 * (input[0] + 1)*0.999f;
            return prev;
        }
    }
}
