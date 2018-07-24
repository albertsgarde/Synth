using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Stuff;
using SynthLib.Data;

namespace SynthLib.Oscillators
{
    /// <summary>
    /// Provides an oscillation between min and max
    /// </summary>
    public abstract class Oscillator : ISaveable
    {
        public abstract string Type { get; }

        public abstract float Frequency { get; set; }

        public abstract int SampleRate { get; }

        protected bool useable = true;

        /// <summary>
        /// Moves the oscillator forward one step. The length of the step is equal to 1 second/SampleRate.
        /// </summary>
        public abstract void Next();

        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>What the value would be, the oscillator oscillated between min and max.</returns>
        public abstract float CurrentValue(float min = -1, float max = 1);

        /// <summary>
        /// A combined call of Next() and CurrentValue(float, float).
        /// Moves the oscillator forward one step, then returns the value.
        /// </summary>
        public float NextValue(float min, float max = 1)
        {
            Next();
            return CurrentValue(min, max);
        }

        public float NextValue(bool doesNothing = false)
        {
            Debug.Assert(useable);
            return NextValue();
        }

        /// <summary>
        /// Like the above, but optimized for min=-1 and max=1;
        /// </summary>
        protected abstract float NextValue();

        public abstract void Reset();

        /// <summary>
        /// Meant as a way of getting more of the same oscillator type.
        /// </summary>
        /// <returns>A deep copy of the Oscillator.</returns>
        public abstract Oscillator Clone(int sampleRate);

        public abstract Oscillator CreateInstance(XElement element, SynthData data);

        public virtual XElement ToXElement(string name)
        {
            Debug.Assert(useable);
            var element = new XElement(name);
            element.AddValue("type", Type);
            return element;
        }
    }
}
