using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Effects
{
    public class Flip : IEffect
    {
        public IEffect Clone()
        {
            return new Flip();
        }

        public float Next(float input)
        {
            return input * -1;
        }
    }
}
