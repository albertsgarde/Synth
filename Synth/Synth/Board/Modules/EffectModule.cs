﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Stuff;
using SynthLib.Effects;

namespace SynthLib.Board.Modules
{
    public class EffectModule : Module
    {
        private IEffect effect;

        public override Connections Inputs { get; }

        public override Connections Outputs { get; }

        public override string Type { get; } = "Effect";

        public EffectModule(IEffect effect)
        {
            this.effect = effect;
            Inputs = new ConnectionsArray(effect.Values + 1, effect.Values);
            Outputs = new ConnectionsArray(1);
        }

        public override Module Clone()
        {
            return new EffectModule(effect.Clone());
        }

        public override float[] Process(float[] inputs, int time, bool noteOn)
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
