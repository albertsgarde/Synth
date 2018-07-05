using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Effects
{
    /// <summary>
    /// Assuming the input is between -1 and 1, it is translated to a value between min and max.
    /// </summary>
    public class Translate : IEffect
    {
        public int Values => 0;

        private readonly float min;

        private readonly float max;

        private readonly float mxmn2;

        private readonly float mxmn2mn;

        public Translate(float min, float max)
        {
            this.min = min;
            this.max = max;
            mxmn2 = (max - min) / 2;
            mxmn2mn = mxmn2 + mxmn2mn;
        }

        public IEffect Clone()
        {
            return new Translate(min, max);
        }

        public float Next(float[] input)
        {
            return input[0] * mxmn2 + mxmn2mn;
        }
    }
}
