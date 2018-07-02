using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Effects
{
    public class Pass : IEffect
    {
        public IEffect Clone()
        {
            return new Pass();
        }

        public float Next(float input)
        {
            return input;
        }
    }
}
