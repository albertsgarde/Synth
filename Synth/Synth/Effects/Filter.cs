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
    public class Filter : Effect
    {
        public override string Type => "Filter";

        private const int VALUES = 0;

        public override int Values => VALUES;

        private readonly float[] impulseResponse;

        private readonly CircularArray<float> prevs;

        public Filter()
        {
            useable = false;
        }

        public Filter(IReadOnlyList<float> impulseResponse)
        {
            this.impulseResponse = impulseResponse.ToArray();
            prevs = new CircularArray<float>(impulseResponse.Count);
        }

        protected override float Next(float[] input)
        {
            prevs.Add(input[VALUES]);
            float result = 0;
            for (int i = 0; i < prevs.Length; ++i)
                result += impulseResponse[i] * prevs[i];
            return result;
        }

        public override Effect Clone()
        {
            return new Filter(impulseResponse);
        }

        public override Effect CreateInstance(XElement element, SynthData data)
        {
            var impulseResponse = new List<float>();
            foreach (var value in element.Element("impulseResponse").Elements())
                impulseResponse.Add(InvalidEffectSaveElementException.ParseFloat(value));
            return new Filter(impulseResponse);
        }

        public override XElement ToXElement(string name)
        {
            var element = base.ToXElement(name);
            var ir = element.CreateElement("impulseResponse");
            for (int i = 0; i < impulseResponse.Length; ++i)
                ir.AddValue("value", impulseResponse[i]);
            return element;
        }
    }
}
