using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Effects
{
    public class SimpleFilter : IEffect
    {
        public float[] prev;

        public SimpleFilter(int numPrevs)
        {
            prev = new float[numPrevs];
        }

        public float Next(float input)
        {
            if (prev.Length == 0)
                return input;

            float result = prev[0];

            for (int i = 1; i < prev.Length; ++i)
            {
                prev[i - 1] = prev[i];
                result += prev[i];
            }
            prev[prev.Length - 1] = input;
            return (result + input) / prev.Length;
        }
    }
}
