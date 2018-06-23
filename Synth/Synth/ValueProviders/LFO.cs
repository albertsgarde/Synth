using SynthLib.Oscillators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.ValueProviders
{
    public class LFO : IValueProvider
    {
        private IOscillator oscillator;

        private float frequency;

        public int SampleRate { get; }

        public LFO(IOscillator oscillator, float frequency, int sampleRate = 44100)
        {
            this.oscillator = oscillator.Clone();
            this.frequency = frequency;
            SampleRate = sampleRate;

            if (oscillator.SampleRate != sampleRate)
                throw new ArgumentException("The oscillator must have the same sample rate as the LFO.");
        }

        public void Next()
        {
            oscillator.Next(frequency);
        }

        public float CurrentValue(float min = -1, float max = 1)
        {
            return oscillator.CurrentValue();
        }

        public float NextValue(float min = -1, float max = 1)
        {
            return oscillator.NextValue(frequency, min, max);
        }
    }
}
