using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff;
using System.Xml.Linq;
using SynthLib.Data;
using System.Diagnostics;
using Stuff.StuffMath;

namespace SynthLib.Effects
{
    public abstract class Effect : ISaveable
    {
        public abstract string Type { get; }

        public abstract int Values { get; }

        protected bool useable = true;

        public abstract Effect Clone();

        public abstract Effect CreateInstance(XElement element, SynthData data);

        public float Next(float[] input, bool doesNothing = true)
        {
            Debug.Assert(useable);
            return Next(input);
        }

        protected abstract float Next(float[] input);

        public virtual XElement ToXElement(string name)
        {
            Debug.Assert(useable);
            var element = new XElement(name);
            element.AddValue("type", Type);
            return element;
        }
    }
}
