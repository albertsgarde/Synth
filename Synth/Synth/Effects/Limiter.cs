using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SynthLib.Data;
using Stuff;

namespace SynthLib.Effects
{
    public class Limiter : Effect
    {
        public override string Type => "Limiter";

        public override int Values => 0;

        public float Limit { get; }

        public Limiter(float limit)
        {
            Limit = limit;
        }

        public override Effect Clone()
        {
            return new Limiter(Limit);
        }

        public override Effect CreateInstance(XElement element, SynthData data)
        {
            var limit = InvalidEffectSaveElementException.ParseFloat(element.Element("limit"));
            return new Boost(limit);
        }

        protected override float Next(float[] input)
        {
            if (input[0] > Limit)
                return Limit;
            else if (input[0] < -Limit)
                return -Limit;
            else
                return input[0];
        }

        public override XElement ToXElement(string name)
        {
            var element = base.ToXElement(name);
            element.AddValue("limit", Limit);
            return element;
        }
    }
}
