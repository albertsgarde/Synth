using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Inputs = new ConnectionsArray(1);
            Outputs = new ConnectionsArray(1);
        }

        public override Module Clone()
        {
            return new EffectModule(effect.Clone());
        }

        public override float[] Process(float[] inputs)
        {
            return new float[] { effect.Next(inputs[0]) };
        }
    }
}
