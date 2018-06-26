using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.ValueProviders
{
    /// <summary>
    /// Provides floats between -1 and 1 for ValueRecievers.
    /// </summary>
    public interface IValueProvider
    {
        int SampleRate { get; }

        /// <summary>
        /// Moves the ValueProvider forward one step. The length of the step is equal to 1 second/SampleRate.
        /// </summary>
        void Next();
        
        /// <returns>The current value.</returns>
        float CurrentValue();

        /// <summary>
        /// A combined call of Next(double) and CurrentValue(float, float).
        /// Moves the ValueProvider forward one step, then returns the value.
        /// </summary>
        float NextValue();
    }
}
