using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SynthLib.Data;

namespace SynthLib.Effects
{
    public class DelegateEffect : Effect
    {
        public override string Type => "Boost";

        public override int Values => 0;

        private Func<float, float> effect;

        public DelegateEffect(Func<float, float> effect)
        {
            this.effect = effect;
        }

        public override Effect Clone()
        {
            return new DelegateEffect(effect);
        }

        public override Effect CreateInstance(XElement element, SynthData data)
        {
            throw new UnsaveableEffectException();
        }

        protected override float Next(float[] input)
        {
            return effect.Invoke(input[0]);
        }

        public override XElement ToXElement(string name)
        {
            throw new UnsaveableEffectException();
        }
    }
}
