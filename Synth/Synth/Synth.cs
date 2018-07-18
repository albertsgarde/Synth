﻿using System;
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

        private readonly Midi midi;
        
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

            //midi.SetMidiIn(0);

            MidiFile file = new MidiFile("D:/PenguinAgen/Documents/Synth/midi/Join_the_Rain-Cello_I.mid");
            var rythm = new MidiFile("D:/PenguinAgen/Documents/Synth/midi/Join_the_Rain-Cello_II.mid");

            midi = new Midi(file.DeltaTicksPerQuarterNote);

            Setup();

        }

        private void Setup()
        {
            var lfo1 = new ConstantOscillatorModule(new SineOscillator(), 3, 1f);

            var env1 = new Envelope(0, 80, 0.8f, 20, 3);

            var t1 = new EffectModule(new Translate(-1.0f, 0));

            var o1 = new OscillatorModule(new SawOscillator(), 1);

            var o2 = new OscillatorModule(new SawOscillator(), 1, 0.08f);

            var o3 = new OscillatorModule(new SawOscillator(), 1, 11.92f);

            var d1 = new Distributer(new float[] { 1, 1, 0.7f }, new float[] { 1 });

            var b1 = new EffectModule(new Boost(1));

            var sf1 = new EffectModule(new SimpleFilter(5));

            var m1 = new Mixer(1, 2);

            var de1 = new EffectModule(new Delay(0.5f, 0.0f));

            var g1 = new EffectModule(new Boost(0.01f));

            var end = new EndModule();

            var board = new BoardTemplate()
            {
                end, m1,  b1, sf1, d1, o3, o2, o1, env1, t1, de1, lfo1, g1
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
            board.AddConnection(sf1, g1);
            
            board.AddConnection(g1, end);

            var superBoard = new PolyBoard(board, 6, 1);
           // midi.NoteOn += superBoard.HandleNoteOn;
            //midi.NoteOff += superBoard.HandleNoteOff;

            board.ToXElement("board").Save("D:/PenguinAgen/Documents/Synth/board.xml");
            SynthUtils.PlayMidiToFile("D:/PenguinAgen/Documents/Synth/midi/SynthTest.mid", "D:/PenguinAgen/Documents/Synth/output/test.wav", superBoard);
            //synthResult.AddSynthProvider(superBoard);


            SynthUtils.PlayMidi("D:/PenguinAgen/Documents/Synth/midi/SynthTest.mid", superBoard, 64);
        }
    }
}
