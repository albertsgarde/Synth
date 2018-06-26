using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynthLib.SynthProviders;
using SynthLib.Oscillators;
using SynthLib.Effects;
using SynthLib.Board.Modules;
using SynthLib.Board;
using NAudio.Midi;
using NAudio.Wave;

namespace SynthLib
{
    public class Synth
    {
        public int SampleRate { get; }

        private readonly SynthResult synthResult;

        private readonly WaveOutEvent aOut;

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

            var midiIn = new MidiIn(0);

            var midi = new Midi(midiIn);

            var o1 = new OscillatorModule(new SawOscillator(), midi, 1);
            var o2 = new OscillatorModule(new SawOscillator(), midi, 1, 0.1f);
            var o3 = new OscillatorModule(new SawOscillator(), midi, 1, 11.9f);

            var d1 = new Distributer(new float[] { 1, 1, 0.4f }, new float[] { 1, 1f });
            var e1 = new EffectModule(new SimpleFilter(5));
            var end = new EndModule();

            Connections.NewConnection(o1, d1);
            Connections.NewConnection(o2, d1);
            Connections.NewConnection(o3, d1);

            Connections.NewConnection(d1, e1);
            Connections.NewConnection(d1, end);

            Connections.NewConnection(e1, end);

            var board = new ModuleBoard()
            {
                end, e1, d1, o3, o2, o1
            };

            synthResult.AddSynthProvider(board);
        }
    }
}
