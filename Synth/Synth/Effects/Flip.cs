using Stuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SynthLib.Effects
{
    public class Flip : IEffect
    {
        public string Type => "Flip";

        public int Values => 0;

        public IEffect Clone()
        {
            return new Flip();
        }

        public float Next(float[] input)
        {
            return input[0] * -1;
        }

        public XElement ToXElement(string name)
        {
            var element = new XElement(name);
            element.AddValue("type", Type);
            return element;
        }
    }
}
