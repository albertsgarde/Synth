using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Stuff;
using SynthLib.Data;

namespace SynthLib.Oscillators
{
    public class CompoundOscillator : Oscillator
    {
        public override string Type => "Compound";

        private readonly Oscillator[] oscillators;

        private float frequency;

        private readonly float totalWeight;

        public override int SampleRate { get; }

        public CompoundOscillator()
        {
            useable = false;
        }

        public CompoundOscillator(IEnumerable<Oscillator> oscillators, int sampleRate = 44100)
        {
            this.oscillators = oscillators.ToArray();
            Frequency = 0;

            totalWeight = 0;
            for (int i = 0; i < this.oscillators.Length; ++i)
                totalWeight += this.oscillators[i].Weight;
            SampleRate = sampleRate;
        }

        public override float Frequency
        {
            get => frequency;
            set
            {
                frequency = value;
                for (int i = 0; i < oscillators.Length; ++i)
                    oscillators[i].UpdateFrequency(frequency);
            }
        }

        public class Oscillator : ISaveable
        {
            private readonly Oscillators.Oscillator oscillator;

            public float Weight { get; }

            private readonly float frequencyMultiplier;

            public Oscillator(Oscillators.Oscillator oscillator, float weight, int halfTones, float cents) : this(oscillator, weight, (float)Math.Pow(2, (1 / 12D) * (halfTones + (cents / 100))))
            {
            }

            private Oscillator(Oscillators.Oscillator oscillator, float weight, float frequencyMultiplier)
            {
                this.oscillator = oscillator;
                Weight = weight;
                this.frequencyMultiplier = frequencyMultiplier;
            }

            public Oscillator(XElement element, SynthData data)
            {
                oscillator = data.OscillatorTypes[element.ElementValue("type")].Instance.CreateInstance(element, data);
                Weight = InvalidOscillatorSaveElementException.ParseFloat(element.Element("weight"));
                frequencyMultiplier = InvalidOscillatorSaveElementException.ParseFloat(element.Element("frequencyMultiplier"));
            }

            public void UpdateFrequency(float frequency)
            {
                oscillator.Frequency = frequency * frequencyMultiplier;
            }

            public void Next()
            {
                oscillator.Next();
            }

            public float CurrentValue(float min, float max)
            {
                return oscillator.CurrentValue(min, max) * Weight;
            }

            public float NextValue(float min, float max)
            {
                return oscillator.NextValue(min, max);
            }

            public float NextValue()
            {
                return oscillator.NextValue();
            }

            public void Reset()
            {
                oscillator.Reset();
            }

            public Oscillator Clone(int sampleRate)
            {
                return new Oscillator(oscillator.Clone(sampleRate), Weight, frequencyMultiplier);
            }

            public XElement ToXElement(string name)
            {
                var element = new XElement(name);
                element.Add(oscillator.ToXElement("osc"));
                element.AddValue("weight", Weight);
                element.AddValue("frequencyMultiplier", frequencyMultiplier);
                return element;
            }
        }

        public override void Next()
        {
            foreach (var osc in oscillators)
                osc.Next();
        }

        public override float CurrentValue(float min = -1, float max = 1)
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

        protected override float NextValue()
        {
            float result = 0;
            for (int i = 0; i < oscillators.Length; ++i)
                result += oscillators[i].NextValue();
            return result / totalWeight;
        }

        public override void Reset()
        {
            foreach (var osc in oscillators)
                osc.Reset();
        }

        public override Oscillators.Oscillator Clone(int sampleRate = 44100)
        {
            var clonedOscillators = new List<Oscillator>();
            foreach (var osc in oscillators.Select(osc => osc.Clone(sampleRate)))
                clonedOscillators.Add(osc);
            return new CompoundOscillator(clonedOscillators, sampleRate);
        }

        public override Oscillators.Oscillator CreateInstance(XElement element, SynthData data)
        {
            var oscillators = new List<Oscillator>();
            foreach (var osc in element.Elements("osc"))
                oscillators.Add(new Oscillator(element, data));
            return new CompoundOscillator(oscillators, data.SampleRate);
        }

        public override XElement ToXElement(string name)
        {
            var element = base.ToXElement(name);
            foreach (var osc in oscillators)
                element.Add(osc.ToXElement("osc"));
            return element;
        }
    }
}
