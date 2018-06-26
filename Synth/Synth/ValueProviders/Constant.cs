using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.ValueProviders
{
    public class Constant : IValueProvider
    {
        private float value;

        public int SampleRate { get; }
        
        public Constant(float value, int sampleRate = 44100) 
        {
            this.value = value;
            SampleRate = sampleRate;
        }

        public void Next()
        {
        }

        public float CurrentValue()
        {
            return value;
        }

        public float NextValue()
        {
            return value;
        }
    }
}
