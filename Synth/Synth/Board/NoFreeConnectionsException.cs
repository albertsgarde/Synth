using SynthLib.Board.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SynthLib.Board
{
    [Serializable()]
    public class NoFreeConnectionsException : Exception
    {
        public Module Module { get; }

        public NoFreeConnectionsException(Module module) : base()
        {
            Module = module;
        }

        public NoFreeConnectionsException(Module module, string message) : base(message)
        {
            Module = module;
        }

        protected NoFreeConnectionsException(SerializationInfo si, StreamingContext sc) : base(si, sc) { }
    }
}
