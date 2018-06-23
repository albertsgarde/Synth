using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.ValueProviders
{
    /// <summary>
    /// Provides floats for modules. A general form of the envelopes and LFOs seen in most subtractive synthesizers.
    /// Should be able to provide values between any two values.
    /// </summary>
    public interface IValueProvider
    {
        int SampleRate { get; }

        /// <summary>
        /// Moves the ValueProvider forward one step. The length of the step is equal to 1 second/SampleRate.
        /// </summary>
        void Next();

        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The current value, resized to min and max.</returns>
        float CurrentValue(float min, float max);

        /// <summary>
        /// A combined call of Next(double) and CurrentValue(float, float).
        /// Moves the ValueProvider forward one step, then returns the value.
        /// </summary>
        float NextValue(float min, float max);
    }
}
