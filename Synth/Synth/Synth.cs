using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynthLib.Oscillators;
using SynthLib.Effects;
using SynthLib.Board.Modules;
using SynthLib.Board;
using SynthLib.Settings;
using SynthLib.MidiSampleProviders;
using NAudio.Midi;
using NAudio.Wave;

namespace SynthLib
{
    public partial class Synth
    {
        public int SampleRate { get; }

        public BoardTemplate Board { get; }

        public SynthResult SynthResult { get; }

        public SynthSettings Settings { get; }

        private readonly Midi midi;
        
        public Synth(SynthSettings settings)
        {
            Settings = settings;

            SampleRate = settings.SampleRate;

            SynthResult = new SynthResult(SampleRate, 1)
            {
                Gain = 1f
            };

            var aOut = new WaveOutEvent
            {
                DesiredLatency = settings.DesiredLatency,
                DeviceNumber = -1
            };
            aOut.Init(SynthResult);
            aOut.Play();

            midi = new Midi(2);
            midi.SetMidiIn(0);

            Board = new BoardTemplate();
            Setup(settings);
        }
    }
}
