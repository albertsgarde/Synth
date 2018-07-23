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
    public class SineOscillator : IOscillator
    {
        public string Type => "Sine";

        public int SampleRate { get; }

        private float frequency;

        private float incrementValue;

        private float curRadians;

        private const float TAU = (float)Math.PI * 2;
        
        /// <param name="startValue">The initial value of the oscillator. Used to avoid noise when switching oscillators.</param>
        public SineOscillator (int sampleRate = 44100)
        {
            curRadians = 0;
            Frequency = 0;
            SampleRate = sampleRate;
        }

        public float Frequency
        {
            get => frequency;
            set
            {
                frequency = value;
                incrementValue = (frequency / SampleRate) * TAU;
            }
        }

        public void Next()
        {
            curRadians += incrementValue;
            curRadians %= TAU;
        }

        public float CurrentValue(float min = -1, float max = 1)
        {
            return ((float)Math.Sin(curRadians) + 1) * (max - min) / 2 + min;
        }

        public float NextValue(float min, float max = 1)
        {
            Next();
            return CurrentValue(min, max);
        }

        public float NextValue()
        {
            Next();
            return (float)Math.Sin(curRadians);
        }

        public void Reset()
        {
            curRadians = 0;
        }

        public IOscillator Clone(int sampleRate = 44100)
        {
            return new SineOscillator(sampleRate);
        }

        public IOscillator CreateInstance(XElement element, SynthData data) => new SineOscillator(data.SampleRate);

        public XElement ToXElement(string name)
        {
            var element = new XElement(name);
            element.AddValue("type", Type);
            return element;
        }
    }
}
