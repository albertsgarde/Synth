using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Board.Modules
{
    public class InvalidModuleValue : Exception
    {
        public Module Module { get; }

        public float Value { get; }

        public InvalidModuleValue(Module module, float value, string message) : base(message)
        {
            Module = module;
            Value = value;
        }
    }
}
