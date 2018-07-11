using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff;
using System.Xml.Linq;

namespace SynthLib.Effects
{
    public interface IEffect : ISaveable
    {
        string Type { get; }

        int Values { get; }

        IEffect Clone();

        float Next(float[] input);
    }
}
