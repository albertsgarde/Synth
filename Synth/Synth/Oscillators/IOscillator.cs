using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Oscillators
{
    /// <summary>
    /// Provides an oscillation between min and max
    /// </summary>
    public interface IOscillator
    {
        float Frequency { get; set; }

        int SampleRate { get; }

        /// <summary>
        /// Moves the oscillator forward one step. The length of the step is equal to 1 second/SampleRate.
        /// </summary>
        void Next();

        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>What the value would be, the oscillator oscillated between min and max.</returns>
        float CurrentValue(float min = -1, float max = 1);

        /// <summary>
        /// A combined call of Next() and CurrentValue(float, float).
        /// Moves the oscillator forward one step, then returns the value.
        /// </summary>
        float NextValue(float min, float max = 1);

        /// <summary>
        /// Like the above, but optimized for min=-1 and max=1;
        /// </summary>
        float NextValue();

        void Reset();

        /// <summary>
        /// Meant as a way of getting more of the same oscillator type.
        /// </summary>
        /// <returns>A deep copy of the Oscillator.</returns>
        IOscillator Clone();
    }
}
