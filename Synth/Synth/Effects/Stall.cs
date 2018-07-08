using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Effects
{
    public class Stall : IEffect
    {
        public int SampleRate { get; }

        public int Values => 0;

        private readonly float[] prevs;

        private int curPrev;

        public Stall(float delaySeconds, int sampleRate = 44100)
        {
            prevs = new float[(int)(delaySeconds * sampleRate)];
            for (int i = 0; i < prevs.Length; ++i)
                prevs[i] = 0;
            curPrev = 0;
        }

        private Stall(Stall stall)
        {
            SampleRate = stall.SampleRate;
            prevs = new float[stall.prevs.Length];
            stall.prevs.CopyTo(prevs, 0);
            curPrev = stall.curPrev;
        }

        public IEffect Clone()
        {
            return new Stall(this);
        }

        public float Next(float[] input)
        {
            float result = prevs[curPrev];
            prevs[curPrev] = input[0];
            curPrev++;
            if (curPrev >= prevs.Length)
                curPrev = 0;
            return result;
        }
    }
}
