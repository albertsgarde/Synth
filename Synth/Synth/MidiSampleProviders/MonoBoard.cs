using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using SynthLib.Board;
using SynthLib.Music;

namespace SynthLib.MidiSampleProviders
{
    public class MonoBoard : IMidiSampleProvider
    {
        private readonly ModuleBoard board;

        private readonly BoardTemplate boardTemplate;

        public int SampleRate { get; }

        public WaveFormat WaveFormat { get; }

        private readonly int channel;

        private float frequency;

        private bool on;

        private float glideTime;

        private List<int> currentTones;

        public MonoBoard(BoardTemplate boardTemplate, int channel, int sampleRate = 44100)
        {
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, 16);
            SampleRate = sampleRate;
            this.channel = channel;
            this.boardTemplate = boardTemplate;
            board = boardTemplate.CreateInstance(sampleRate);
            frequency = 0;
            on = false;
            currentTones = new List<int>();
        }

        public IMidiSampleProvider Clone()
        {
            return new MonoBoard(boardTemplate, channel, SampleRate);
        }

        public void HandleNoteOn(int noteNumber)
        {
            board.NoteOn(noteNumber);
            if (currentTones.Contains(noteNumber))
                currentTones.Remove(noteNumber);
            currentTones.Add(noteNumber);
            frequency = (float)Tone.FrequencyFromNote(noteNumber);
            on = true;
        }

        public void HandleNoteOff(int noteNumber)
        {
            currentTones.Remove(noteNumber);
            if (board.Note == noteNumber)
            {
                if (on = (currentTones.Count != 0))
                    board.NoteOn(currentTones.Last());
                else
                    board.NoteOff();
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            for (int i = 0; i < count; ++i)
                buffer[i] = board.Next();
            return count;
        }
    }
}
