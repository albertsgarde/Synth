using Stuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SynthLib.Effects
{
    public class SimpleFilter : IEffect
    {
        public string Type => "SimpleFilter";

        public int Values => 0;

        public float[] prev;

        public SimpleFilter(int numPrevs)
        {
            prev = new float[numPrevs];
        }

        public IEffect Clone()
        {
            return new SimpleFilter(prev.Length);
        }

        public float Next(float[] input)
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

        public XElement ToXElement(string name)
        {
            var element = new XElement(name);
            element.AddValue("type", Type);
            element.AddValue("numPrevs", prev.Length);
            return element;
        }
    }
}
