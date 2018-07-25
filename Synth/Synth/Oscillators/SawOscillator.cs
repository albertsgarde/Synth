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
    public class SawOscillator : Oscillator
    {
        public override string Type => "Saw";

        private float frequency;
        private float valueIncrement;

        public override int SampleRate { get; }

        private float curValue;

        public SawOscillator() : this(44100)
        {
        }

        public SawOscillator(int sampleRate)
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

        public override float CurrentValue(float min = -1, float max = 1)
        {
            return curValue * (max - min) + min;
        }

        protected override float NextValue()
        {
            Next();
            return curValue;
        }

        public override void Reset()
        {
            curValue = 0;
        }

        public override Oscillator Clone(int sampleRate = 44100)
        {
            return new SawOscillator(sampleRate);
        }

        public override Oscillator CreateInstance(XElement element, SynthData data)
        {
            return new SawOscillator(data.SampleRate);
        }
    }
}
