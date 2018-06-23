using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynthLib.SynthProviders;
using SynthLib.Oscillators;
using SynthLib.Effects;
using NAudio.Midi;
using NAudio.Wave;

namespace SynthLib
{
    public class Synth
    {
        public int SampleRate { get; }

        private SynthResult synthResult;

        private WaveOutEvent aOut;

        public Synth(int sampleRate = 44100)
        {
            SampleRate = sampleRate;

            synthResult = new SynthResult(sampleRate, 1)
            {
                Gain = 0.1f
            };

            var aOut = new WaveOutEvent
            {
                DesiredLatency = 64
            };
            aOut.Init(synthResult);
            aOut.Play();

            Setup();
        }

        private void Setup()
        {
            var oscillators = new List<CompoundOscillator.Oscillator>()
            {
                new CompoundOscillator.Oscillator(new SawOscillator(), 0.4f, 0, 0),
                new CompoundOscillator.Oscillator(new SawOscillator(), 0.4f, 0, 10),
                new CompoundOscillator.Oscillator(new SawOscillator(), 0.16f, 12, -10),
            };
            var compOsc = new CompoundOscillator(oscillators, SampleRate);

            var midiIn = new MidiIn(0);
            var keyboard = new KeyboardPlayer(midiIn, compOsc);
            synthResult.AddSynthProvider(keyboard);
            synthResult.AddEffect(new SimpleFilter(5));
        }
    }
}
