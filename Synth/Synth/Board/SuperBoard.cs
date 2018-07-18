using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynthLib.SynthProviders;
using SynthLib.Music;
using Stuff;

namespace SynthLib.Board
{
    public class SuperBoard : ISynthProvider
    {
        private readonly ModuleBoard[] boards;

        private readonly BoardTemplate boardTemplate;

        private readonly int voices;

        public int SampleRate { get; }

        public bool Finished { get; private set; }

        public SuperBoard(BoardTemplate boardTemplate, int voices, int sampleRate = 44100)
        {
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

        public float[] Next(int samples)
        {
            float[] result = new float[samples];
            for (int i = 0; i < samples; ++i)
                result[i] = 0;
            Parallel.ForEach(boards, b =>
            {
                for (int i = 0; i < samples; ++i)
                {
                    var next = b.Next();
                    lock (result)
                        result[i] += next;
                }
            });

            /*foreach (var b in boards)
            {
                for (int i = 0; i < samples; ++i)
                {
                    result[i] += b.Next();
                }
            }*/
            return result;
        }
    }
}
