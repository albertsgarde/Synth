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
    public class Flip : Effect
    {
        public override string Type => "Flip";

        public override int Values => 0;

        protected override float Next(float[] input)
        {
            return input[0] * -1;
        }

        public override Effect Clone()
        {
            return new Flip();
        }

        public override Effect CreateInstance(XElement element, SynthData data)
        {
            return new Flip();
        }
    }
}
