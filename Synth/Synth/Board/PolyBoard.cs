using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynthLib.Music;
using Stuff;
using NAudio.Wave;

namespace SynthLib.Board
{
    public class PolyBoard : IMidiSampleProvider
    {
        private readonly ModuleBoard[] boards;

        private readonly BoardTemplate boardTemplate;

        private readonly int voices;

        private readonly int channel;

        public int SampleRate { get; }

        public bool Finished { get; private set; }

        public WaveFormat WaveFormat { get; }

        public PolyBoard(BoardTemplate boardTemplate, int voices, int channel, int sampleRate = 44100)
        {
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channel);
            this.channel = channel;
            SampleRate = sampleRate;
            Finished = false;
            this.boardTemplate = boardTemplate;
            this.voices = voices;
            boards = new ModuleBoard[voices];

            for (int i = 0; i < voices; ++i)
                boards[i] = boardTemplate.CreateInstance();
        }

        public void HandleNoteOn(int noteNumber)
        {
            if (boards.Count(mb => !mb.IsNoteOn) > 0)
                boards.Where(mb => !mb.IsNoteOn).MaxValue(mb => mb.Time).NoteOn(noteNumber);
            else
                boards.MaxValue(mb => mb.Time).NoteOn(noteNumber);
        }

        public void HandleNoteOff(int noteNumber)
        {
            foreach (var mb in boards)
            {
                if (mb.Note == noteNumber)
                    mb.NoteOff();
            }
        }

        public IMidiSampleProvider Clone()
        {
            return new PolyBoard(boardTemplate, voices, channel, SampleRate);
        }

        public int Read(float[] buffer, int offset, int count)
        {
            for (int i = 0; i < count; ++i)
                buffer[i] = 0;
            Parallel.ForEach(boards, b =>
            {
                for (int i = 0; i < count; ++i)
                {
                    var next = b.Next();
                    lock (buffer)
                        buffer[i] += next;
                }
            });

            /*foreach (var b in boards)
            {
                for (int i = 0; i < samples; ++i)
                {
                    result[i] += b.Next();
                }
            }*/
            return count;
        }
    }
}
