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
    /// Delays the signal, but does not repeat it like the delay effect does.
    /// </summary>
    public class Stall : Effect
    {
        public override string Type => "Stall";

        public int SampleRate { get; }

        public override int Values => 0;

        private readonly float[] prevs;

        private readonly float delaySeconds;

        private readonly int delaySamples;

        private int curPrev;

        public Stall()
        {
            useable = false;
        }

        public Stall(float delaySeconds, int sampleRate = 44100) : this((int)(delaySeconds * sampleRate), sampleRate)
        {

        }

        public Stall(int delaySamples, int sampleRate = 44100)
        {
            SampleRate = sampleRate;
            this.delaySamples = delaySamples;
            delaySeconds = (float)delaySamples / sampleRate;
            prevs = new float[delaySamples];
            for (int i = 0; i < prevs.Length; ++i)
                prevs[i] = 0;
            curPrev = 0;
        }

        private Stall(Stall stall)
        {
            delaySeconds = stall.delaySeconds;
            delaySamples = stall.delaySamples;
            SampleRate = stall.SampleRate;
            prevs = new float[stall.prevs.Length];
            stall.prevs.CopyTo(prevs, 0);
            curPrev = stall.curPrev;
        }

        protected override float Next(float[] input)
        {
            float result = prevs[curPrev];
            prevs[curPrev] = input[0];
            curPrev++;
            if (curPrev >= prevs.Length)
                curPrev = 0;
            return result;
        }

        public override Effect Clone()
        {
            return new Stall(this);
        }

        public override Effect CreateInstance(XElement element, SynthData data)
        {
            var delaySeconds = InvalidEffectSaveElementException.ParseFloat(element.Element("delaySeconds"));
            return new Stall(delaySeconds, data.SampleRate);
        }

        public override XElement ToXElement(string name)
        {
            var element = new XElement(name);
            element.AddValue("type", Type);
            element.AddValue("delaySeconds", delaySeconds);
            return element;
        }
    }
}
