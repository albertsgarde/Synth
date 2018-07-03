using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Oscillators
{
    public class SineOscillator : IOscillator
    {
        public int SampleRate { get; }

        private float frequency;

        private float incrementValue;

        private float curRadians;

        private const float TAU = (float)Math.PI * 2;
        
        /// <param name="startValue">The initial value of the oscillator. Used to avoid noise when switching oscillators.</param>
        public SineOscillator (float frequency, float startValue = 0, int sampleRate = 44100)
        {
            curRadians = (float)Math.Asin(startValue);
            SampleRate = sampleRate;
            Frequency = frequency;
        }

        public float Frequency
        {
            get => frequency;
            set
            {
                frequency = value;
                incrementValue = frequency / SampleRate * TAU;
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

        public IOscillator Clone()
        {
            return new SineOscillator((float)Math.Sin(curRadians), SampleRate);
        }
    }
}
