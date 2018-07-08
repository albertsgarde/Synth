using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Oscillators
{
    public class SawOscillator : IOscillator
    {
        private float frequency;
        private float valueIncrement;

        public int SampleRate { get; }

        private float curValue;

        public SawOscillator(float frequency, float startValue = 0, int sampleRate = 44100)
        {
            Frequency = frequency;
            curValue = startValue;
            SampleRate = sampleRate;
        }

        public float Frequency
        {
            get => frequency;
            set
            {
                frequency = value;
                valueIncrement = frequency / SampleRate;
            }
        }

        public void Next()
        {
            curValue += valueIncrement;
            curValue %= 1;
        }

        public float CurrentValue(float min = -1, float max = 1)
        {
            return curValue * (max - min) + min;
        }
        
        public float NextValue(float min, float max = 1)
        {
            Next();
            return CurrentValue(min, max);
        }

        public float NextValue()
        {
            Next();
            return curValue;
        }

        public void Reset()
        {
            curValue = 0;
        }

        public IOscillator Clone()
        {
            return new SawOscillator(frequency, curValue, SampleRate);
        }
    }
}
