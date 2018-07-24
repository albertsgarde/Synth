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
    public class PulseOscillator : Oscillator
    {
        public override string Type => "";

        public override int SampleRate { get; }

        private float frequency;

        private float incrementValue;

        private float curValue = 0;

        private readonly float dutyCycle;

        public PulseOscillator()
        {
            useable = false;
        }

        public PulseOscillator(float dutyCycle = 0.5f, int sampleRate = 44100)
        {
            SampleRate = sampleRate;
            Frequency = 0;
            this.dutyCycle = dutyCycle;
        }

        public override float Frequency
        {
            get => frequency;
            set
            {
                frequency = value;
                incrementValue = frequency / SampleRate;
            }
        }

        public override void Next()
        {
            curValue += incrementValue;
            curValue %= 1;
        }

        public override float CurrentValue(float min = -1, float max = 1)
        {
            return curValue < dutyCycle ? min : max;
        }

        protected override float NextValue()
        {
            Next();
            if (curValue < dutyCycle)
                return -1;
            else
                return 1;
        }

        public override void Reset()
        {
            curValue = 0;
        }

        public override Oscillator Clone(int sampleRate = 44100)
        {
            return new PulseOscillator(dutyCycle, sampleRate);
        }

        public override Oscillator CreateInstance(XElement element, SynthData data)
        {
            var dutyCycle = InvalidOscillatorSaveElementException.ParseFloat(element.Element("dutyCycle"));
            return new PulseOscillator(dutyCycle, data.SampleRate);
        }

        public override XElement ToXElement(string name)
        {
            var element = base.ToXElement(name);
            element.AddValue("dutyCycle", dutyCycle);
            return element;
        }
    }
}
