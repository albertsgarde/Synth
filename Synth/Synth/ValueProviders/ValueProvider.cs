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
    public abstract class ValueProvider
    {
        public abstract int SampleRate { get; }

        public delegate void ValueUpdateHandler(float value);

        public event ValueUpdateHandler ValueUpdated;

        public void Next()
        {
            ValueUpdated(NextValue());
        }

        protected abstract float NextValue();
    }
}
