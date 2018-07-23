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
    /// Assuming the input is between -1 and 1, it is translated to a value between min and max.
    /// </summary>
    public class Translate : Effect
    {
        public override string Type => "Translate";

        public override int Values => 0;

        private readonly float min;

        private readonly float max;

        private readonly float mxmn2;

        private readonly float mxmn2mn;

        public Translate()
        {
            useable = false;
        }

        public Translate(float min, float max)
        {
            this.min = min;
            this.max = max;
            mxmn2 = (max - min) / 2;
            mxmn2mn = mxmn2 + min;
        }

        protected override float Next(float[] input)
        {
            return input[0] * mxmn2 + mxmn2mn;
        }

        public override Effect Clone()
        {
            return new Translate(min, max);
        }

        public override Effect CreateInstance(XElement element, SynthData data)
        {
            var min = InvalidEffectSaveElementException.ParseFloat(element.Element("min"));
            var max = InvalidEffectSaveElementException.ParseFloat(element.Element("max"));
            return new Translate(min, max);
        }

        public override XElement ToXElement(string name)
        {
            var element = new XElement(name);
            element.AddValue("type", Type);
            element.AddValue("min", min);
            element.AddValue("max", max);
            return element;
        }
    }
}
