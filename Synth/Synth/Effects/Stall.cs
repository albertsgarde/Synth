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
    /// Delays the signal, but does not repeat it like the delay effect does.
    /// </summary>
    public class Stall : IEffect
    {
        public string Type => "Stall";

        public int SampleRate { get; }

        public int Values => 0;

        private readonly float[] prevs;

        private readonly float delaySeconds;

        private int curPrev;

        public Stall(float delaySeconds, int sampleRate = 44100)
        {
            this.delaySeconds = delaySeconds;
            prevs = new float[(int)(delaySeconds * sampleRate)];
            for (int i = 0; i < prevs.Length; ++i)
                prevs[i] = 0;
            curPrev = 0;
        }

        private Stall(Stall stall)
        {
            delaySeconds = stall.delaySeconds;
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

        public XElement ToXElement(string name)
        {
            var element = new XElement(name);
            element.AddValue("type", Type);
            element.AddValue("delaySeconds", delaySeconds);
            return element;
        }
    }
}
