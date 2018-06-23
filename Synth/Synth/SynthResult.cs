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

        private readonly List<IEffect> addEffects;

        private readonly List<IEffect> effects;

        private readonly List<IEffect> removeEffects;

        public WaveFormat WaveFormat { get; private set; }

        public float Gain { get; set; }

        public SynthResult(int sampleRate, int channel)
        {
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channel);
            addSynthProviders = new List<ISynthProvider>();
            synthProviders = new List<ISynthProvider>();
            effects = new List<IEffect>();
            addEffects = new List<IEffect>();
            removeEffects = new List<IEffect>();
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

        public void AddEffect(IEffect e)
        {
            lock (addEffects)
                addEffects.Add(e);
        }

        public void RemoveEffect(IEffect e)
        {
            lock (removeEffects)
                removeEffects.Add(e);
        }

        private void AddRemoveEffects()
        {
            lock (addEffects)
            {
                effects.AddRange(addEffects);
                addEffects.Clear();
            }
            lock (removeEffects)
            {
                foreach (var effect in removeEffects)
                    effects.Remove(effect);
                removeEffects.Clear();
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            AddProviders();
            synthProviders.RemoveAll(sp => sp.Finished);
            AddRemoveEffects();


            var bufferCount = 0;
            for (var outI = 0; outI < count; outI++)
            {
                float sample = 0;
                foreach (var sp in synthProviders)
                    sample += sp.Next();
                foreach (var effect in effects)
                    sample = effect.Next(sample);
                buffer[bufferCount++] = sample * Gain;

            }
            return count;
        }
    }
}
