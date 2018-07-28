using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynthLib.Oscillators;
using SynthLib.Effects;
using SynthLib.Board.Modules;
using SynthLib.Board;
using SynthLib.Data;
using SynthLib.MidiSampleProviders;
using NAudio.Midi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace SynthLib
{
    public partial class Synth
    {
        public int SampleRate { get; }

        private BoardTemplate board;
        
        public SynthResult SynthResult { get; }

        public SynthData Data { get; }

        private readonly Midi midi;

        public Synth(SynthData data)
        {
            Data = data;

            SampleRate = Data.SampleRate;

            SynthResult = new SynthResult(data)
            {
                Gain = 1f
            };

            var aOut = new WaveOutEvent
            {
                DesiredLatency = data.DesiredLatency,
                DeviceNumber = -1
            };

            aOut.Init(SynthResult);
            aOut.Play();

            midi = new Midi(2);
            midi.SetMidiIn(0);

            board = new BoardTemplate();
            SetupBoard(Data);
            Setup(Data);
        }

        partial void SetupBoard(SynthData data);

        /// <summary>
        /// Called every time the Board is set.
        /// </summary>
        partial void Setup(SynthData data);

        public BoardTemplate Board
        {
            get => board;
            set
            {
                board = value;
                Setup(Data);
            }
        }
    }
}
