using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib
{
    public class SynthSettings
    {
        public int SampleRate { get; }
        public int DesiredLatency { get; }

        public SynthSettings(int sampleRate = 44100, int desiredLatency = 64)
        {
            SampleRate = sampleRate;
            DesiredLatency = desiredLatency;
        }
    }
}
