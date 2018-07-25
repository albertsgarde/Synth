using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;
using SynthLib.Board.Modules;
using Stuff;
using System.Xml.Linq;

namespace SynthLib.Board
{
    public class Connection
    {
        public Module Source { get; private set; }

        public int SourceIndex { get; private set; }

        public Module Destination { get; private set; }

        public int DestinationIndex { get; private set; }

        /// <summary>
        /// Creates an active connection between two modules.
        /// </summary>
        /// <param name="sourceModule">The source module.</param>
        /// <param name="sourceOutputIndex">Which of the source module's output indexes to use.</param>
        /// <param name="destModule">The destination module.</param>
        /// <param name="destInputIndex">Which of the destination module's input indexes to use</param>
        public Connection(Module sourceModule, int sourceOutputIndex, Module destModule, int destInputIndex)
        {
            Source = sourceModule;
            SourceIndex = sourceOutputIndex;
            Destination = destModule;
            DestinationIndex = destInputIndex;
        }

        public void Validate()
        {
            Debug.Assert(Source.Outputs[SourceIndex] == this, "Connection invalid between modules of types " + Source.GetType().Name + " and " + Destination.GetType().Name + ".");
            Debug.Assert(Destination.Inputs[DestinationIndex] == this, "Connection invalid between modules of types " + Source.GetType().Name + " and " + Destination.GetType().Name + ".");
        }

        public void Destroy()
        {
            Connections.Destroy(this);
        }
    }
}
