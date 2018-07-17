using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Music
{
    public class Key
    {
        public Scale Scale { get; }
        
        public Tone Root { get; }

        public Key (Tone note, Scale scale)
        {
            Root = note;
            Scale = scale;
        }

        public Tone Step(int step, int modifier)
        {
            return Root + Scale.Step(step) + modifier;
        }
    }
}
