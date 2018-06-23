using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Oscillators
{
    /// <summary>
    /// Provides an oscillation between -1 and 1
    /// </summary>
    public interface IOscillator
    {
        int SampleRate { get; }

        /// <returns>The next frame given the current frequency.</returns>
        float Next(double frequency);

        void Reset();

        /// <summary>
        /// Meant as a way of getting more of the same oscillator type.
        /// </summary>
        /// <returns>A deep copy of the Oscillator.</returns>
        IOscillator Clone();
    }
}
