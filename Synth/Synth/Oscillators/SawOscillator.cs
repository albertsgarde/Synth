using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Oscillators
{
    public class SawOscillator : IOscillator
    {
        public int SampleRate { get; }

        private double curValue ;

        public SawOscillator(double startValue = 0, int sampleRate = 44100)
        {
            curValue = (startValue + 1) / 2;
            SampleRate = sampleRate;
        }

        public void Next(double frequency)
        {
            curValue += frequency / SampleRate;
            curValue %= 1;
        }

        public float CurrentValue(float min = -1, float max = 1)
        {
            return (float)curValue * (max - min) + min;
        }
        
        public float NextValue(double frequency, float min = -1, float max = 1)
        {
            Next(frequency);
            return CurrentValue(min, max);
        }

        public void Reset()
        {
            curValue = 0;
        }

        public IOscillator Clone()
        {
            return new SawOscillator(curValue * 2 - 1, SampleRate);
        }
    }
}
