using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Stuff;
using SynthLib.Data;

namespace SynthLib.Effects.Filters
{
    public class LowPassFilter : Effect
    {
        public override string Type => "LowPassFilter";

        public override int Values => 0; // 0: Cutoff frequency.

        private const int SUPER_KERNEL_SIZE = 441000;

        private static readonly float[] superKernel;

        private readonly float cutoffFrequency;

        static LowPassFilter()
        {
            superKernel = new float[SUPER_KERNEL_SIZE];
        }

        public LowPassFilter(float cutoffFrequency)
        {
            this.cutoffFrequency = cutoffFrequency;
        }

        public override Effect Clone()
        {
            return new LowPassFilter(cutoffFrequency);
        }

        public override Effect CreateInstance(XElement element, SynthData data)
        {
            return new LowPassFilter(InvalidEffectSaveElementException.ParseFloat(element.Element("cutoffFrequency")));
        }

        protected override float Next(float[] input)
        {
            throw new NotImplementedException();
        }

        public override XElement ToXElement(string name)
        {
            var element = base.ToXElement(name);
            element.AddValue("cutoffFrequency", cutoffFrequency);
            return element;
        }
    }
}
