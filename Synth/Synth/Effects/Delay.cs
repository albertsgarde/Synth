using Stuff;
using SynthLib.Data;
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
    public class Delay : Effect
    {
        public override string Type => "Delay";

        public override int Values => 1;

        private readonly float[] prevs;

        private readonly float delaySeconds;

        private int curPrev;

        private readonly float feedback;

        public Delay()
        {
            useable = false;
        }

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

        public override Effect Clone()
        {
            return new Delay(this);
        }

        public override Effect CreateInstance(XElement element, SynthData data)
        {
            var delaySeconds = InvalidEffectSaveElementException.ParseFloat(element.Element("delaySeconds"));
            var feedback = InvalidEffectSaveElementException.ParseFloat(element.Element("feedback"));
            return new Delay(delaySeconds, feedback, data.SampleRate);
        }

        protected override float Next(float[] input)
        {
            prevs[curPrev] = prevs[curPrev] * feedback * (input[0] + 1) + input[1];
            float result = prevs[curPrev];
            if (++curPrev >= prevs.Length)
                curPrev = 0;
            return result;
        }

        public override XElement ToXElement(string name)
        {
            var element = base.ToXElement(name);
            element.AddValue("delaySeconds", delaySeconds);
            element.AddValue("feedback", feedback);
            return element;
        }
    }
}
