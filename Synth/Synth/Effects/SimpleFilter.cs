using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Effects
{
    public class SimpleFilter : IEffect
    {
        public int Values => 0;

        public float[] prev;

        public SimpleFilter(int numPrevs)
        {
            prev = new float[numPrevs];
        }

        public IEffect Clone()
        {
            return new SimpleFilter(prev.Length);
        }

        public float Next(float[] input)
        {
            if (prev.Length == 0)
                return input[0];

            float result = prev[0];

            for (int i = 1; i < prev.Length; ++i)
            {
                prev[i - 1] = prev[i];
                result += prev[i];
            }
            prev[prev.Length - 1] = input[0];
            return (result + input[0]) / prev.Length;
        }
    }
}
