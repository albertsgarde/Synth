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
        private readonly List<IMidiSampleProvider> addSynthProviders;

        private readonly List<IMidiSampleProvider> synthProviders;

        public WaveFormat WaveFormat { get; private set; }

        public float Gain { get; set; }

        public SynthResult(int sampleRate, int channel)
        {
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channel);
            addSynthProviders = new List<IMidiSampleProvider>();
            synthProviders = new List<IMidiSampleProvider>();
            Gain = 1;
        }

        public void AddSynthProvider(IMidiSampleProvider sp)
        {
            lock (addSynthProviders)
                addSynthProviders.Add(sp);
        }

        private void AddProviders()
        {
            lock (addSynthProviders)
            {
                synthProviders.AddRange(addSynthProviders);
                addSynthProviders.Clear();
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            AddProviders();

            var spResults = new List<float[]>();
            foreach (var sp in synthProviders)
                spResults.Add(sp.Next(count));

            var bufferCount = 0;
            for (var outI = 0; outI < count; outI++)
            {
                float sample = 0;
                foreach (var spr in spResults)
                    sample += spr[outI];
                buffer[bufferCount++] = sample * Gain;

            }
            return count;
        }
    }
}
