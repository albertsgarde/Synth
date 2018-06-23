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
        int SampleRate { get; }

        /// <summary>
        /// Moves the oscillator forward one step. The length of the step is equal to 1 second/SampleRate.
        /// </summary>
        void Next(double frequency);

        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>What the value would be, the oscillator oscillated between min and max.</returns>
        float CurrentValue(float min = -1, float max = 1);

        /// <summary>
        /// A combined call of Next(double) and CurrentValue(float, float).
        /// Moves the oscillator forward one step, then returns the value.
        /// </summary>
        float NextValue(double frequency, float min, float max);

        void Reset();

        /// <summary>
        /// Meant as a way of getting more of the same oscillator type.
        /// </summary>
        /// <returns>A deep copy of the Oscillator.</returns>
        IOscillator Clone();
    }
}
