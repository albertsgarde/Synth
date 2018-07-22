using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Stuff;

namespace SynthLib.Oscillators
{
    public class CustomOscillator : IOscillator
    {
        public string Type => "Custom";

        public int SampleRate { get; }

        private readonly IReadOnlyList<float> values;

        private readonly float valueLength;

        private float position;

        private int curValue;

        private float posIncrement;
        private float frequency;

        public CustomOscillator(IReadOnlyList<float> values, float valueLength, int sampleRate = 44100)
        {
            SampleRate = sampleRate;
            Frequency = Frequency;
            this.values = values;
            this.valueLength = valueLength;
            position = 0;
            curValue = 0;
        }

        public float Frequency
        {
            get => frequency;
            set
            {
                frequency = value;
                posIncrement = valueLength * SampleRate / frequency;
            }
        }

        public IOscillator Clone()
        {
            return new CustomOscillator(values, valueLength, SampleRate);
        }

        private float CurrentValue()
        {
            return values[curValue] + (values[curValue + 1] - values[curValue]) * position;
        }

        public float CurrentValue(float min = -1F, float max = 1)
        {
            return (CurrentValue() + 1) * (max - min) / 2 + min;
        }

        public void Next()
        {
            position += posIncrement;
            if (position > 1)
            {
                ++curValue;
                position -= 1;
            }

        }

        public float NextValue(float min, float max = 1)
        {
            Next();
            return CurrentValue(min, max);
        }

        public float NextValue()
        {
            Next();
            return CurrentValue();
        }

        public void Reset()
        {
            position = 0;
            curValue = 0;
        }

        public XElement ToXElement(string name)
        {
            var element = new XElement(name);
            element.AddValue("type", Type);
            element.AddValue("valueLength", valueLength);
            var valuesElement = element.CreateElement("values");
            foreach (var value in values)
                valuesElement.AddValue("value", value);
            return element;
        }
    }
}
