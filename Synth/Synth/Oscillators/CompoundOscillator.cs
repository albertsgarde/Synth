using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Oscillators
{
    public class CompoundOscillator : IOscillator
    {
        private readonly IEnumerable<Oscillator> oscillators;

        public int SampleRate { get; }

        public CompoundOscillator(IEnumerable<Oscillator> oscillators, int sampleRate = 44100)
        {
            this.oscillators = oscillators;
            SampleRate = sampleRate;
        }

        public class Oscillator
        {
            private readonly IOscillator oscillator;

            public float Weight { get; private set; }

            private readonly double frequencyMultiplier;

            public Oscillator(IOscillator oscillator, float weight, int halfTones, float cents) : this(oscillator, weight, Math.Pow(2, (1 / 12D) * (halfTones + (cents / 100))))
            {
            }

            private Oscillator(IOscillator oscillator, float weight, double frequencyMultiplier)
            {
                this.oscillator = oscillator;
                Weight = weight;
                this.frequencyMultiplier = frequencyMultiplier;
            }

            public void Next(double frequency)
            {
                oscillator.Next(frequency * frequencyMultiplier);
            }

            public float CurrentValue(float min, float max)
            {
                return oscillator.CurrentValue(min, max) * Weight;
            }

            public float NextValue(double frequency, float min, float max)
            {
                return oscillator.NextValue(frequency, min, max);
            }

            public void Reset()
            {
                oscillator.Reset();
            }

            public Oscillator Clone()
            {
                return new Oscillator(oscillator.Clone(), Weight, frequencyMultiplier);
            }
        }

        public void Next(double frequency)
        {
            foreach (var osc in oscillators)
                osc.Next(frequency);
        }

        public float CurrentValue(float min = -1, float max = 1)
        {
            float result = 0;
            float totalWeight = 0;
            foreach (var osc in oscillators)
            {
                result += osc.CurrentValue(min, max);
                totalWeight += osc.Weight;
            }
            return result / totalWeight;
        }

        public float NextValue(double frequency, float min = -1, float max = 1)
        {
            float result = 0;
            float totalWeight = 0;
            foreach (var osc in oscillators)
            {
                result += osc.NextValue(frequency, min, max);
                totalWeight += osc.Weight;
            }
            return result / totalWeight;
        }

        public void Reset()
        {
            foreach (var osc in oscillators)
                osc.Reset();
        }

        public IOscillator Clone()
        {
            var clonedOscillators = new List<Oscillator>();
            foreach (var osc in oscillators.Select(osc => osc.Clone()))
                clonedOscillators.Add(osc);
            return new CompoundOscillator(clonedOscillators, SampleRate);
        }
    }
}
