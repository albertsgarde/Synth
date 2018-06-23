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

        public void Next(double frequency)
        {
            curValue += frequency / SampleRate;
            curValue %= 1;
        }

        public float CurrentValue(float min = -1, float max = 1)
        {
            return curValue < 0.5 ? min : max;
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
            return new SquareOscillator(SampleRate);
        }
    }
}
