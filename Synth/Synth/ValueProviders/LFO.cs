using SynthLib.Oscillators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.ValueProviders
{
    public class LFO : ValueProvider
    {
        private readonly IOscillator oscillator;

        private readonly float frequency;

        private readonly float min;

        private readonly float max;

        public override int SampleRate { get; }

        public LFO(IOscillator oscillator, float frequency, float min = -1, float max = 1, int sampleRate = 44100)
        {
            if (min < -1 || min > 1)
                throw new ArgumentException("Min out of range. Required: -1 <= min <= 1");
            if (max < -1 || max > 1)
                throw new ArgumentException("Max out of range. Required: -1 <= min <= 1");
            this.oscillator = oscillator.Clone();
            this.frequency = frequency;
            this.min = min;
            this.max = max;
            SampleRate = sampleRate;

            if (oscillator.SampleRate != sampleRate)
                throw new ArgumentException("The oscillator must have the same sample rate as the LFO.");
        }

        protected override float NextValue()
        {
            return (oscillator.NextValue(frequency) + 1) *(max - min) / 2 + min;
        }
    }
}
