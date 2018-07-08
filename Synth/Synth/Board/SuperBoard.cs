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

        private readonly Midi midi;

        private readonly int voices;

        public int SampleRate { get; }

        public bool Finished { get; private set; }

        public SuperBoard(BoardTemplate boardTemplate, Midi midi, int voices, int sampleRate = 44100)
        {
            SampleRate = sampleRate;
            Finished = false;
            this.boardTemplate = boardTemplate;
            this.midi = midi;
            this.voices = voices;
            boards = new ModuleBoard[voices];

            for (int i = 0; i < voices; ++i)
                boards[i] = boardTemplate.CreateInstance();

            midi.NoteOn += HandleNoteOn;
            midi.NoteOff += HandleNoteOff;
        }

        private void HandleNoteOn(int noteNumber)
        {
            if (boards.Count(mb => !mb.IsNoteOn) > 0)
                boards.Where(mb => !mb.IsNoteOn).MaxValue(mb => mb.Time).NoteOn(noteNumber);
            else
                boards.MaxValue(mb => mb.Time).NoteOn(noteNumber);
        }

        private void HandleNoteOff(int noteNumber)
        {
            foreach (var mb in boards)
            {
                if (mb.Note == noteNumber)
                    mb.NoteOff();
            }
        }

        public float Next()
        {
            float result = 0;
            foreach (var board in boards)
            {
                result += board.Next();
            }
            return result;
        }
    }
}
