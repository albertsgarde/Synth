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
    public class CustomOscillator : Oscillator
    {
        public override string Type => "Custom";

        public override int SampleRate { get; }

        private readonly IReadOnlyList<float> values;

        private readonly float valueLength;

        private float position;

        private int curValue;

        private float posIncrement;
        private float frequency;

        public CustomOscillator()
        {
            useable = false;
        }

        public CustomOscillator(IReadOnlyList<float> values, float valueLength, int sampleRate = 44100)
        {
            SampleRate = sampleRate;
            Frequency = Frequency;
            this.values = values;
            this.valueLength = valueLength;
            position = 0;
            curValue = 0;
        }

        public override float Frequency
        {
            get => frequency;
            set
            {
                frequency = value;
                posIncrement = valueLength * SampleRate / frequency;
            }
        }

        private float CurrentValue()
        {
            return values[curValue] + (values[curValue + 1] - values[curValue]) * position;
        }

        public override float CurrentValue(float min = -1F, float max = 1)
        {
            return (CurrentValue() + 1) * (max - min) / 2 + min;
        }

        public override void Next()
        {
            position += posIncrement;
            if (position > 1)
            {
                ++curValue;
                position -= 1;
            }

        }

        protected override float NextValue()
        {
            Next();
            return CurrentValue();
        }

        public override void Reset()
        {
            position = 0;
            curValue = 0;
        }

        public override Oscillator Clone(int sampleRate = 44100)
        {
            return new CustomOscillator(values, valueLength, sampleRate);
        }

        public override Oscillator CreateInstance(XElement element, SynthData data)
        {
            var valueLength = InvalidOscillatorSaveElementException.ParseFloat(element.Element("valueLength"));
            var values = new List<float>();
            foreach (var valueElement in element.Element("values").Elements())
            {
                values.Add(InvalidOscillatorSaveElementException.ParseFloat(valueElement));
            }
            return new CustomOscillator(values, valueLength, data.SampleRate);
        }

        public override XElement ToXElement(string name)
        {
            var element = base.ToXElement(name);
            element.AddValue("valueLength", valueLength);
            var valuesElement = element.CreateElement("values");
            foreach (var value in values)
                valuesElement.AddValue("value", value);
            return element;
        }
    }
}
