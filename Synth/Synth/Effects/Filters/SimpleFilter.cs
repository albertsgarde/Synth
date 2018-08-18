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
    public class SimpleFilter : Effect
    {
        public override string Type => "SimpleFilter";

        public override int Values => 0;

        public float[] prev;

        public SimpleFilter()
        {
            useable = false;
        }

        public SimpleFilter(int numPrevs)
        {
            prev = new float[numPrevs];
        }

        protected override float Next(float[] input)
        {
            if (prev.Length == 0)
                return input[0];

            float result = prev[0];

            for (int i = 1; i < prev.Length; ++i)
            {
                prev[i - 1] = prev[i];
                result += prev[i];
            }
            prev[prev.Length - 1] = input[0];
            return (result + input[0]) / prev.Length;
        }

        public override Effect Clone()
        {
            return new SimpleFilter(prev.Length);
        }

        public override Effect CreateInstance(XElement element, SynthData data)
        {
            var numPrevs = InvalidEffectSaveElementException.ParseInt(element.Element("numPrevs"));
            return new SimpleFilter(numPrevs);
        }

        public override XElement ToXElement(string name)
        {
            var element = base.ToXElement(name);
            element.AddValue("numPrevs", prev.Length);
            return element;
        }
    }
}
