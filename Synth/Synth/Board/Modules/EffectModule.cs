using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Stuff;
using SynthLib.Data;
using SynthLib.Effects;

namespace SynthLib.Board.Modules
{
    public class EffectModule : Module
    {
        private Effect effect;

        public override Connections Inputs { get; }

        public override Connections Outputs { get; }

        public override BoardOutput OutputType { get; } = BoardOutput.None;

        public EffectModule()
        {
            useable = false;
        }

        public EffectModule(Effect effect)
        {
            this.effect = effect;
            Inputs = new ConnectionsArray(effect.Values + 1, effect.Values);
            Outputs = new ConnectionsArray(1);
        }

        public EffectModule(XElement element, SynthData data)
        {
            effect = data.EffectTypes[element.Element("effect").ElementValue("type")].Instance.CreateInstance(element.Element("effect"), data);
            Inputs = new ConnectionsArray(element.Element("inputs"));
            Outputs = new ConnectionsArray(element.Element("outputs"));
        }

        public override Module Clone(int sampleRate = 44100)
        {
            return new EffectModule(effect.Clone());
        }

        public override Module CreateInstance(XElement element, SynthData data)
        {
            return new EffectModule(element, data);
        }

        protected override float[] IntProcess(float[] inputs, long time, bool noteOn)
        {
            return new float[] { effect.Next(inputs) };
        }

        public override XElement ToXElement(string name)
        {
            var element = base.ToXElement(name);
            element.Add(effect.ToXElement("effect"));
            return element;
        }
    }
}
