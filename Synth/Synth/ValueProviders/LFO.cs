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
        private readonly IOscillator oscillator;

        private readonly float frequency;

        public int SampleRate { get; }

        public LFO(IOscillator oscillator, float frequency, float min = -1, float max = 1, int sampleRate = 44100)
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

        public float CurrentValue()
        {
            return oscillator.CurrentValue();
        }

        public float NextValue()
        {
            return oscillator.NextValue(frequency);
        }
    }
}
