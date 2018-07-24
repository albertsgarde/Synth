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
    public class Pass : Effect
    {
        public override string Type => "Pass";

        public override int Values => 0;

        protected override float Next(float[] input)
        {
            return input[0];
        }

        public override Effect Clone()
        {
            return new Pass();
        }

        public override Effect CreateInstance(XElement element, SynthData data)
        {
            return new Pass();
        }
    }
}
