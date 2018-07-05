using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using SynthLib.Music;
using SynthLib.Oscillators;
using SynthLib.SynthProviders;
using SynthLib.Effects;
using Stuff;

namespace SynthLib
{
    public class SynthResult : ISampleProvider
    {
        private readonly List<ISynthProvider> addSynthProviders;

        private readonly List<ISynthProvider> synthProviders;

        public WaveFormat WaveFormat { get; private set; }

        public float Gain { get; set; }

        public SynthResult(int sampleRate, int channel)
        {
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channel);
            addSynthProviders = new List<ISynthProvider>();
            synthProviders = new List<ISynthProvider>();
            Gain = 1;
        }

        public void AddSynthProvider(ISynthProvider sp)
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
            synthProviders.RemoveAll(sp => sp.Finished);


            var bufferCount = 0;
            for (var outI = 0; outI < count; outI++)
            {
                float sample = 0;
                foreach (var sp in synthProviders)
                    sample += sp.Next();
                buffer[bufferCount++] = sample * Gain;

            }
            return count;
        }
    }
}
