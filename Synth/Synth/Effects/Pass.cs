using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Stuff;

namespace SynthLib.Effects
{
    public class Pass : IEffect
    {
        public string Type => "Pass";

        public int Values => 0;

        public IEffect Clone()
        {
            return new Pass();
        }

        public float Next(float[] input)
        {
            return input[0];
        }

        public XElement ToXElement(string name)
        {
            var element = new XElement(name);
            element.AddValue("type", Type);
            return element;
        }
    }
}
