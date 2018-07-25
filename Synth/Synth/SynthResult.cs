using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using SynthLib.Music;
using SynthLib.Oscillators;
using SynthLib.Effects;
using Stuff;
using SynthLib.MidiSampleProviders;

namespace SynthLib
{
    public class SynthResult : ISampleProvider
    {
        private IMidiSampleProvider synthProvider;

        private IMidiSampleProvider newSynthProvider;

        public WaveFormat WaveFormat { get; private set; }

        public float Gain { get; set; }

        public SynthResult(int sampleRate)
        {
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, 2);
            synthProvider = null;
            newSynthProvider = null;
            Gain = 1;
        }

        public void ReplaceSynthProvider(IMidiSampleProvider synthProvider)
        {
            newSynthProvider = synthProvider;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            if (newSynthProvider != null)
            {
                lock (newSynthProvider)
                {
                    synthProvider = newSynthProvider;
                    newSynthProvider = null;
                }
            }
            if (synthProvider != null)
            {
                synthProvider.Next(buffer, offset, count, Gain);
            }

            return count;
        }
    }
}
