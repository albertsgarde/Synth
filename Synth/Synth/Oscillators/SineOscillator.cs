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
    public class SineOscillator : Oscillator
    {
        public override string Type => "Sine";

        public override int SampleRate { get; }

        private float frequency;

        private float incrementValue;

        private float curRadians;

        private const float TAU = (float)Math.PI * 2;

        public SineOscillator() : this(44100)
        {
        }

        /// <param name="startValue">The initial value of the oscillator. Used to avoid noise when switching oscillators.</param>
        public SineOscillator (int sampleRate = 44100)
        {
            SampleRate = sampleRate;
            curRadians = 0;
            Frequency = 0;
        }

        public override float Frequency
        {
            get => frequency;
            set
            {
                frequency = value;
                incrementValue = (frequency / SampleRate) * TAU;
            }
        }

        public override void Next()
        {
            curRadians += incrementValue;
            curRadians %= TAU;
        }

        public override float CurrentValue(float min = -1, float max = 1)
        {
            return ((float)Math.Sin(curRadians) + 1) * (max - min) / 2 + min;
        }

        protected override float NextValue()
        {
            Next();
            return (float)Math.Sin(curRadians);
        }

        public override void Reset()
        {
            curRadians = 0;
        }

        public override Oscillator Clone(int sampleRate = 44100)
        {
            return new SineOscillator(sampleRate);
        }

        public override Oscillator CreateInstance(XElement element, SynthData data) => new SineOscillator(data.SampleRate);
    }
}
