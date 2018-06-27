using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Oscillators
{
    public class PulseOscillator : IOscillator
    {
        public int SampleRate { get; }

        private double curValue = 0;

        private double dutyCycle;

        public PulseOscillator(double dutyCycle = 0.5, int sampleRate = 44100)
        {
            SampleRate = sampleRate;
            this.dutyCycle = dutyCycle;
        }

        public double DutyCycle
        {
            get
            {
                return dutyCycle;
            }

            set
            {
                if (value > 1 || value < 0)
                    throw new ArgumentException("Duty cycle must be between 0 and 1.");
                else
                    dutyCycle = value;
            }
        }

        public void Next(double frequency)
        {
            curValue += frequency / SampleRate;
            curValue %= 1;
        }

        public float CurrentValue(float min = -1, float max = 1)
        {
            return curValue < dutyCycle ? min : max;
        }

        public float NextValue(double frequency, float min = -1, float max = 1)
        {
            Next(frequency);
            return CurrentValue(min, max);
        }

        public void Reset()
        {
            curValue = 0;
        }

        public IOscillator Clone()
        {
            return new PulseOscillator(SampleRate);
        }
    }
}
