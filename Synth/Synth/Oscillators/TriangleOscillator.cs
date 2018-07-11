using Stuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SynthLib.Oscillators
{
    public class TriangleOscillator : IOscillator
    {
        public string Type => "Triangle";

        public int SampleRate { get; }

        private float frequency;

        private float valueIncrement;

        private float curValue;

        public TriangleOscillator(float startValue = 0, int sampleRate = 44100)
        {
            SampleRate = sampleRate;
            Frequency = 0;
            curValue = startValue;
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

        public float CurrentValue(float min = -1F, float max = 1)
        {
            float result;
            if (curValue <= 0.5f)
                result = curValue;
            else
                result = 0.5f - curValue;
            return result * (max - min) + min;
        }

        public float NextValue(float min, float max = 1)
        {
            Next();
            return CurrentValue(min, max);
        }

        public float NextValue()
        {
            Next();
            return curValue <= 0.5f ? curValue : 0.5f - curValue;
        }

        public IOscillator Clone()
        {
            return new TriangleOscillator(curValue, SampleRate);
        }

        public void Reset()
        {
            curValue = 0;
        }

        public XElement ToXElement(string name)
        {
            var element = new XElement(name);
            element.AddValue("type", Type);
            return element;
        }
    }
}
