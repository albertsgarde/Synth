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
    public class Synth
    {
        public int SampleRate { get; }

        private BoardTemplate boardTemplate;

        private IMidiSampleProvider msp;

        private Func<BoardTemplate, IMidiSampleProvider> midiSampleProviderCreator;

        private readonly SynthResult synthResult;

        public (float left, float right) MaxValue => synthResult.MaxValue;

        public SynthData Data { get; }

        private readonly Midi midi;

        public Synth(SynthData data)
        {
            Data = data;

            SampleRate = Data.SampleRate;

            synthResult = new SynthResult(data)
            {
                Gain = 1f
            };



            var aOut = new WaveOutEvent
            {
                DesiredLatency = data.DesiredLatency,
                DeviceNumber = FindAudioDevice()
            };

            aOut.Init(synthResult);
            aOut.Play();

            midi = new Midi(240);
            for (int i = 0; i < MidiIn.NumberOfDevices; ++i)
            {
                if (MidiIn.DeviceInfo(i).ProductName == "MPKmini2")
                    midi.SetMidiIn(i);
            }

            msp = new NullProvider();
            msp.SubscribeToMidi(midi);
            boardTemplate = SynthSetup.SetupBoard(Data);
            MidiSampleProviderCreator = SynthSetup.DefaultMidiSampleProviderCreator(Data);
        }

        public int FindAudioDevice()
        {
            for (int n = 0; n < WaveOut.DeviceCount; n++)
            {
                var caps = WaveOut.GetCapabilities(n);
                if (caps.ProductName == "Speakers (3- AudioBox USB 96)")
                    return n;
            }
            return -1;
        }

        public BoardTemplate BoardTemplate
        {
            get => boardTemplate;
            set
            {
                boardTemplate = value;
                ReplaceSampleProvider();
            }
        }

        public Func<BoardTemplate, IMidiSampleProvider> MidiSampleProviderCreator
        {
            get => midiSampleProviderCreator;
            set
            {
                midiSampleProviderCreator = value;
                ReplaceSampleProvider();
            }
        }

        private void ReplaceSampleProvider()
        {
            msp.UnsubscribeFromMidi(midi);
            
            msp = midiSampleProviderCreator.Invoke(boardTemplate);

            msp.SubscribeToMidi(midi);

            synthResult.ReplaceSynthProvider(msp);
        }
    }
}
