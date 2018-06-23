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

        private double curRadians;

        private const double TAU = Math.PI * 2;
        
        /// <param name="startValue">The initial value of the oscillator. Used to avoid noise when switching oscillators.</param>
        public SineOscillator (double startValue = 0, int sampleRate = 44100)
        {
            curRadians = Math.Asin(startValue);
            SampleRate = sampleRate;
        }

        public float Next(double frequency)
        {
            curRadians += frequency / SampleRate * TAU;
            curRadians %= TAU;
            return (float)Math.Sin(curRadians);
        }

        public void Reset()
        {
            curRadians = 0;
        }

        public IOscillator Clone()
        {
            return new SineOscillator(Math.Sin(curRadians), SampleRate);
        }
    }
}
