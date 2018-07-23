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
    public class RandomOscillator : IOscillator
    {
        public string Type => "Random";

        private float frequency;

        public int SampleRate { get; }

        private float curValue;

        private float incrementValue;

        private float loopValue;

        private readonly int seed;

        private Random rand;

        public RandomOscillator(int sampleRate = 44100) : this((int)DateTime.Now.Ticks, sampleRate)
        {
        }

        public RandomOscillator(int seed, int sampleRate = 44100)
        {
            SampleRate = sampleRate;
            Frequency = 0;
            this.seed = seed;
            rand = new Random(seed);
            curValue = (float)(rand.NextDouble() * 2 - 1);
            loopValue = 0;
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

        public IOscillator Clone(int sampleRate = 44100)
        {
            return new RandomOscillator(seed);
        }

        public IOscillator CreateInstance(XElement element, SynthData data)
        {
            var seed = InvalidOscillatorSaveElementException.ParseInt(element.Element("seed"));
            return new RandomOscillator(seed, data.SampleRate);
        }

        public float CurrentValue(float min = -1F, float max = 1)
        {
            return curValue * (max - min) + min;
        }

        public void Next()
        {
            loopValue += incrementValue;
            if (loopValue > 1)
            {
                loopValue -= 1;
                curValue = (float)(rand.NextDouble() * 2 - 1);
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
            return curValue;
        }

        public void Reset()
        {
            loopValue = 0;
            rand = new Random(seed);
        }

        public XElement ToXElement(string name)
        {
            var element = new XElement(name);
            element.AddValue("type", Type);
            element.AddValue("seed", seed);
            return element;
        }
    }
}
