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

            public float Next(double frequency)
            {
                return oscillator.Next(frequency * frequencyMultiplier) * Weight;
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

        public float Next(double frequency)
        {
            float result = 0;
            float totalWeight = 0;
            foreach (var osc in oscillators)
            {
                result += osc.Next(frequency);
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
