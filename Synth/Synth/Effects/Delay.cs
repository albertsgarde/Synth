using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Effects
{
    /// <summary>
    /// The traditional delay effect.
    /// </summary>
    public class Delay : IEffect
    {
        public int Values => 0;

        private readonly float[] prevs;

        private int curPrev;

        private readonly float feedback;

        public Delay(float delaySeconds, float feedback, float sampleRate = 44100)
        {
            prevs = new float[(int)(delaySeconds * sampleRate)];
            for (int i = 0; i < prevs.Length; ++i)
                prevs[i] = 0;
            curPrev = 0;
            this.feedback = feedback;
        }

        private Delay(Delay delay)
        {
            prevs = new float[delay.prevs.Length];
            delay.prevs.CopyTo(prevs, 0);
            curPrev = delay.curPrev;
            feedback = delay.feedback;
        }

        public IEffect Clone()
        {
            return new Delay(this);
        }

        public float Next(float[] input)
        {
            prevs[curPrev] = prevs[curPrev] * feedback + input[0];
            float result = prevs[curPrev];
            if (++curPrev >= prevs.Length)
                curPrev = 0;
            return result;
        }
    }
}
