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
    public class TriangleOscillator : Oscillator
    {
        public override string Type => "Triangle";

        public override int SampleRate { get; }

        private float frequency;

        private float valueIncrement;

        private float curValue;

        public TriangleOscillator() : this(44100)
        {

        }

        public TriangleOscillator(int sampleRate = 44100)
        {
            SampleRate = sampleRate;
            Frequency = 0;
            curValue = 0;
        }

        public override float Frequency
        {
            get => frequency;
            set
            {
                frequency = value;
                valueIncrement = frequency / SampleRate;
            }
        }

        public override void Next()
        {
            curValue += valueIncrement;
            curValue %= 1;
        }

        public override float CurrentValue(float min = -1F, float max = 1)
        {
            float result;
            if (curValue <= 0.5f)
                result = curValue;
            else
                result = 0.5f - curValue;
            return result * (max - min) + min;
        }

        protected override float NextValue()
        {
            Next();
            return curValue <= 0.5f ? curValue : 0.5f - curValue;
        }

        public override Oscillator Clone(int sampleRate = 44100)
        {
            return new TriangleOscillator(sampleRate);
        }

        public override Oscillator CreateInstance(XElement element, SynthData data) => new TriangleOscillator(data.SampleRate);

        public override void Reset()
        {
            curValue = 0;
        }
    }
}
