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

        private float frequency;

        private float incrementValue;

        private float curValue = 0;

        private float dutyCycle;

        public PulseOscillator(float frequency, float dutyCycle = 0.5f, int sampleRate = 44100)
        {
            SampleRate = sampleRate;
            Frequency = frequency;
            this.dutyCycle = dutyCycle;
        }

        public float Frequency
        {
            get => frequency;
            set
            {
                frequency = value;
                incrementValue = frequency / SampleRate;
            }
        }

        public float DutyCycle
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

        public void Next()
        {
            curValue += incrementValue;
            curValue %= 1;
        }

        public float CurrentValue(float min = -1, float max = 1)
        {
            return curValue < dutyCycle ? min : max;
        }

        public float NextValue(float min = -1, float max = 1)
        {
            Next();
            return CurrentValue(min, max);
        }

        public float NextValue()
        {
            Next();
            if (curValue < dutyCycle)
                return -1;
            else
                return 1;
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
