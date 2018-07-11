using Stuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SynthLib.Effects
{
    /// <summary>
    /// The traditional delay effect.
    /// </summary>
    public class Delay : IEffect
    {
        public string Type => "Delay";

        public int Values => 1;

        private readonly float[] prevs;

        private readonly float delaySeconds;

        private int curPrev;

        private readonly float feedback;

        public Delay(float delaySeconds, float feedback, float sampleRate = 44100)
        {
            this.delaySeconds = delaySeconds;
            prevs = new float[(int)(delaySeconds * sampleRate)];
            for (int i = 0; i < prevs.Length; ++i)
                prevs[i] = 0;
            curPrev = 0;
            this.feedback = feedback;
        }

        private Delay(Delay delay)
        {
            delaySeconds = delay.delaySeconds;
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
            prevs[curPrev] = prevs[curPrev] * feedback * (input[0] + 1) + input[1];
            float result = prevs[curPrev];
            if (++curPrev >= prevs.Length)
                curPrev = 0;
            return result;
        }

        public XElement ToXElement(string name)
        {
            var element = new XElement(name);
            element.AddValue("type", Type);
            element.AddValue("delaySeconds", delaySeconds);
            element.AddValue("feedback", feedback);
            return element;
        }
    }
}
