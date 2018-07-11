using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Stuff;

namespace SynthLib.Board.Modules
{
    public abstract class Module : ISaveable
    {
        public abstract Connections Inputs { get; }

        public abstract Connections Outputs { get; }

        public abstract string Type { get; }

        public abstract float[] Process(float[] inputs, int time, bool noteOn);

        /// <summary>
        /// Used by module boards. Should not be touched by anything else.
        /// </summary>
        public int num;

        public virtual void UpdateFrequency(float frequency)
        {
            
        }

        public IEnumerable<Connection> Connections()
        {
            return Inputs.Concat(Outputs);
        }

        /// <summary>
        /// The clone should be complete apart from the connections, which should be empty.
        /// </summary>
        /// <returns>A clone of the called module.</returns>
        public abstract Module Clone();

        /// <summary>
        /// Starts the construction of an XElement that describes the module. Should be called at the start of any Modules ToXElement implementation.
        /// Saves data about connections and type, the rest should be saved by the individual module.
        /// </summary>
        public virtual XElement ToXElement(string name)
        {
            var element = new XElement(name);
            element.AddValue("type", Type);
            element.Add(Inputs.ToXElement("inputs"));
            element.Add(Outputs.ToXElement("outputs"));
            return element;
        }
    }
}
