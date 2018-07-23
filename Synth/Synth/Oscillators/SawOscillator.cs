using Stuff;
using SynthLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SynthLib.Oscillators
{
    public class SawOscillator : IOscillator
    {
        public string Type => "Saw";

        private float frequency;
        private float valueIncrement;

        public int SampleRate { get; }

        private float curValue;

        public SawOscillator(int sampleRate = 44100)
        {
            Frequency = 0;
            curValue = 0;
            SampleRate = sampleRate;
        }

        public float Frequency
        {
            get => frequency;
            set
            {
                frequency = value;
                valueIncrement = frequency / SampleRate;
            }
        }

        public void Next()
        {
            curValue += valueIncrement;
            curValue %= 1;
        }

        public float CurrentValue(float min = -1, float max = 1)
        {
            return curValue * (max - min) + min;
        }
        
        public float NextValue(float min, float max = 1)
        {
            Next();
            return CurrentValue(min, max);
        }

        public float NextValue()
        {
            Next();
            return curValue;
        }

        public void Reset()
        {
            curValue = 0;
        }

        public IOscillator Clone(int sampleRate = 44100)
        {
            return new SawOscillator(sampleRate);
        }

        public IOscillator CreateInstance(XElement element, SynthData data)
        {
            return new SawOscillator(data.SampleRate);
        }

        public XElement ToXElement(string name)
        {
            var element = new XElement(name);
            element.AddValue("type", Type);
            return element;
        }
    }
}
