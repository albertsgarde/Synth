using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.ValueProviders
{
    public class Constant : ValueProvider
    {
        private readonly float value;

        public override int SampleRate { get; }
        
        public Constant(float value, int sampleRate = 44100) 
        {
            this.value = value;
            SampleRate = sampleRate;
        }

        protected override float NextValue()
        {
            return value;
        }
    }
}
