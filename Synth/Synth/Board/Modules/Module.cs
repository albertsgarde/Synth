using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml.Linq;
using Stuff;
using SynthLib.Data;

namespace SynthLib.Board.Modules
{
    /// <summary>
    /// What kind of output this module provides.
    /// </summary>
    public enum BoardOutput
    {
        /// <summary>
        /// This module's output[0] is added to the board's left output channel.
        /// </summary>
        Left,
        /// <summary>
        /// This module's output[0] is added to the board's right output channel.
        /// </summary>
        Right,
        /// <summary>
        /// This module's output[0] is multplied with the board's glide channel.
        /// </summary>
        GlideTime,
        /// <summary>
        /// This module's output[0] is multiplied by the board's pitch wheel range and added to the boards tone.
        /// </summary>
        PitchShift,
        /// <summary>
        /// This module has no special board hook.
        /// </summary>
        None
    }

    public abstract class Module : ISaveable
    {
        public abstract Connections Inputs { get; }

        public abstract Connections Outputs { get; }

        /// <summary>
        /// What kind of output this module provides.
        /// </summary>
        public abstract BoardOutput OutputType { get; }

        protected bool useable = true;

        public float[] Process(float[] inputs, long time, bool noteOn, ModuleBoard moduleBoard)
        {
            Debug.Assert(useable);
            return IntProcess(inputs, time, noteOn, moduleBoard);
        }

        protected abstract float[] IntProcess(float[] inputs, long time, bool noteOn, ModuleBoard moduleBoard);

        /// <summary>
        /// Used by module boards. Should not be touched by anything else.
        /// </summary>
        public int num;

        public virtual void UpdateFrequency(float frequency)
        {
            Debug.Assert(useable);
        }

        public IEnumerable<Connection> Connections()
        {
            Debug.Assert(useable);
            return Inputs.Concat(Outputs);
        }

        /// <summary>
        /// The clone should be complete apart from the connections, which should be empty.
        /// </summary>
        /// <returns>A clone of the called module.</returns>
        public abstract Module Clone(int sampleRate = 44100);

        public abstract Module CreateInstance(XElement element, SynthData data);

        /// <summary>
        /// Starts the construction of an XElement that describes the module. Should be called at the start of any Modules ToXElement implementation.
        /// Saves data about connections and type, the rest should be saved by the individual module.
        /// </summary>
        public virtual XElement ToXElement(string name)
        {
            Debug.Assert(useable);
            var element = new XElement(name);
            element.AddValue("type", GetType().Name);
            element.Add(Inputs.ToXElement("inputs"));
            element.Add(Outputs.ToXElement("outputs"));
            return element;
        }
    }
}
