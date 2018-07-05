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
            MidiIn midiIn;
            try
            {
                 midiIn = new MidiIn(0);
            }
            catch (Exception e)
            {
                Console.WriteLine(MidiIn.NumberOfDevices);
                throw e;
            }

            var midi = new Midi(midiIn);

            var lfo1 = new ConstantOscillatorModule(new SineOscillator(2f), 3, 0.5f);

            var o1 = new OscillatorModule(new SawOscillator(0), 1);

            var o2 = new OscillatorModule(new SawOscillator(0), 1, 0.1f);

            var o3 = new OscillatorModule(new SawOscillator(0), 1, 11.9f);

            var d1 = new Distributer(new float[] { 1, 1, 0.4f }, new float[] { 1 });

            var b1 = new EffectModule(new Boost(1));

            var m1 = new Mixer(1, 2);

            var sf1 = new EffectModule(new SimpleFilter(5));
            var m2 = new Mixer(2, 1);
            m1.OutputGains[0] = 0.2f;
            var end = new EndModule();

            var board = new BoardTemplate()
            {
                end, m1, m2, b1, sf1, d1, o3, o2, o1, lfo1
            };

            board.AddConnection(lfo1, b1, destIndex: 0);

            board.AddConnection(o1, d1);
            board.AddConnection(o2, d1);
            board.AddConnection(o3, d1);

            board.AddConnection(d1, b1);
            board.AddConnection(b1, m1);
            board.AddConnection(m1, sf1);
            board.AddConnection(m1, m2);
            
            board.AddConnection(sf1, m2);
            
            board.AddConnection(m2, end);

            var superBoard = new SuperBoard(board, midi);

            synthResult.AddSynthProvider(superBoard);
        }
    }
}
