using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Stuff;
using SynthLib.Data;

namespace SynthLib.Effects
{
    public class OnePoleFilter : Effect
    {
        public override int Values => 1;

        private float prev;

        private readonly float a1;

        private const float MAX_A1 = 0.999f;

        public override string Type => "OnePoleFilter";

        public OnePoleFilter()
        {
            useable = false;
        }

        public OnePoleFilter(float a1)
        {
            prev = 0;
            this.a1 = a1;
        }

        protected override float Next(float[] input)
        {
            prev = input[Values] + prev * -a1 * (input[0] + 1) * 0.999f;
            return prev;
        }

        public override Effect Clone()
        {
            return new OnePoleFilter(a1);
        }

        public override Effect CreateInstance(XElement element, SynthData data)
        {
            var a1 = InvalidEffectSaveElementException.ParseFloat(element.Element("a1"));
            return new OnePoleFilter(a1);
        }

        public override XElement ToXElement(string name)
        {
            var element = base.ToXElement(name);
            element.AddValue("a1", a1);
            return element;
        }
    }
}
