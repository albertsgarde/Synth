using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.SynthProviders
{
    public interface ISynthProvider
    {
        int SampleRate { get; }

        /// <summary>
        /// The ISynthProvider has no more to give when this is true.
        /// </summary>
        bool Finished { get; }

        float Next();
    }
}
