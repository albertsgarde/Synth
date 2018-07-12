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
        
        public Synth(SynthSettings settings)
        {
            SampleRate = settings.SampleRate;

            synthResult = new SynthResult(SampleRate, 1)
            {
                Gain = 0.1f
            };

            var aOut = new WaveOutEvent
            {
                DesiredLatency = settings.DesiredLatency,
                DeviceNumber = -1
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

            var lfo1 = new ConstantOscillatorModule(new SineOscillator(), 1, 1f);

            var env1 = new Envelope(30, 80, 0.8f, 40, 3);

            var t1 = new EffectModule(new Translate(-1.0f, 0));

            var o1 = new OscillatorModule(new SawOscillator(), 1);

            var o2 = new OscillatorModule(new SawOscillator(), 1, 0.1f);

            var o3 = new OscillatorModule(new SawOscillator(), 1, 11.9f);

            var d1 = new Distributer(new float[] { 1, 1, 0.4f }, new float[] { 1 });

            var b1 = new EffectModule(new Boost(1));

            var sf1 = new EffectModule(new SimpleFilter(5));

            var m1 = new Mixer(1, 2);

            var de1 = new EffectModule(new Delay(0.5f, 0.5f));

            var m2 = new Mixer(2, 1);
            var end = new EndModule();

            var board = new BoardTemplate()
            {
                end, m1, m2, b1, sf1, d1, o3, o2, o1, env1, t1, de1, lfo1
            };

            //board.AddConnection(lfo1, t1);
            //board.AddConnection(t1, de1, destIndex: 0);

            board.AddConnection(env1, o1, destIndex: 0);
            board.AddConnection(env1, o2, destIndex: 0);
            board.AddConnection(env1, o3, destIndex: 0);

            board.AddConnection(o1, d1);
            board.AddConnection(o2, d1);
            board.AddConnection(o3, d1);

            board.AddConnection(d1, b1);
            board.AddConnection(b1, sf1);
            board.AddConnection(sf1, de1);
            
            board.AddConnection(de1, end);

            var superBoard = new SuperBoard(board, midi, 36);

            board.ToXElement("board").Save("D:/PenguinAgen/Documents/Synth/board.xml");

            synthResult.AddSynthProvider(superBoard);
        }
    }
}
