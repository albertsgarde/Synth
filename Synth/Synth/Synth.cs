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
using SynthLib.ValueProviders;
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

            var o1 = new OscillatorModule(new SawOscillator(), 1);

            var o2 = new OscillatorModule(new PulseOscillator(), 1, 0.1f);

            var o3 = new OscillatorModule(new SawOscillator(), 1, 11.9f);

            var d1 = new Distributer(new float[] { 1, 1, 0.4f, 0.4f, 0.4f }, new float[] { 1, 1f });
            var e1 = new EffectModule(new SimpleFilter(5));
            var m1 = new Mixer(2, 1);
            m1.OutputGains[0] = 0.2f;
            var end = new EndModule();

            Connections.NewConnection(o1, d1);
            Connections.NewConnection(o2, d1);
            Connections.NewConnection(o3, d1);

            Connections.NewConnection(d1, e1);
            Connections.NewConnection(d1, m1);

            Connections.NewConnection(e1, m1);

            Connections.NewConnection(m1, end);

            var board = new BoardTemplate()
            {
                end, m1, e1, d1, o3, o2, o1
            };

            board.AddConnection(o1, d1);
            board.AddConnection(o2, d1);
            board.AddConnection(o3, d1);

            board.AddConnection(d1, e1);
            board.AddConnection(d1, m1);
            
            board.AddConnection(e1, m1);
            
            board.AddConnection(m1, end);

            var superBoard = new SuperBoard(board, midi);

            synthResult.AddSynthProvider(superBoard);
        }
    }
}
