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
    public class PulseOscillator : IOscillator
    {
        public string Type => "Pulse";

        public int SampleRate { get; }

        private float frequency;

        private float incrementValue;

        private float curValue = 0;

        private readonly float dutyCycle;

        public PulseOscillator(float dutyCycle = 0.5f, int sampleRate = 44100)
        {
            SampleRate = sampleRate;
            Frequency = 0;
            this.dutyCycle = dutyCycle;
        }

        public float Frequency
        {
            get => frequency;
            set
            {
                frequency = value;
                incrementValue = frequency / SampleRate;
            }
        }

        public void Next()
        {
            curValue += incrementValue;
            curValue %= 1;
        }

        public float CurrentValue(float min = -1, float max = 1)
        {
            return curValue < dutyCycle ? min : max;
        }

        public float NextValue(float min = -1, float max = 1)
        {
            Next();
            return CurrentValue(min, max);
        }

        public float NextValue()
        {
            Next();
            if (curValue < dutyCycle)
                return -1;
            else
                return 1;
        }

        public void Reset()
        {
            curValue = 0;
        }

        public IOscillator Clone(int sampleRate = 44100)
        {
            return new PulseOscillator(dutyCycle, sampleRate);
        }

        public IOscillator CreateInstance(XElement element, SynthData data)
        {
            var dutyCycle = InvalidOscillatorSaveElementException.ParseFloat(element.Element("dutyCycle"));
            return new PulseOscillator(dutyCycle, data.SampleRate);
        }

        public XElement ToXElement(string name)
        {
            var element = new XElement(name);
            element.AddValue("type", Type);
            element.AddValue("dutyCycle", dutyCycle);
            return element;
        }
    }
}
