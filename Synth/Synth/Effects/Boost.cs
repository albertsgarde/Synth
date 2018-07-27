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
    public class Boost : Effect
    {
        public override string Type => "Boost";

        public override int Values => 1; // 0: Gain modifier;

        public float Gain { get; }

        public Boost()
        {
            useable = false;
        }

        public Boost(float gain, bool decibel = false)
        {
            Gain = decibel ? (float)NAudio.Utils.Decibels.DecibelsToLinear(gain) : gain;
        }

        public override Effect Clone()
        {
            return new Boost(Gain);
        }

        public override Effect CreateInstance(XElement element, SynthData data)
        {
            var gain = InvalidEffectSaveElementException.ParseFloat(element.Element("gain"));
            return new Boost(gain);
        }

        protected override float Next(float[] input)
        {
            return input[Values] * Gain * (input[0] + 1);
        }

        public override XElement ToXElement(string name)
        {
            var element = base.ToXElement(name);
            element.AddValue("gain", Gain);
            return element;
        }
    }
}
