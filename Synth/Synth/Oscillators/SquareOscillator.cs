using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Oscillators
{
    public class SquareOscillator : IOscillator
    {
        public int SampleRate { get; }

        private double curValue = 0;

        public SquareOscillator(int sampleRate = 44100)
        {
            SampleRate = sampleRate;
        }

        public float Next(double frequency)
        {
            curValue += frequency / SampleRate;
            curValue %= 1;
            return CurrentValue();
        }

        public float CurrentValue()
        {
            return curValue < 0.5 ? 0 : 1;
        }

        public void Reset()
        {
            curValue = 0;
        }

        public IOscillator Clone()
        {
            return new SquareOscillator(SampleRate);
        }
    }
}
