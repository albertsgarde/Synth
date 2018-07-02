using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynthLib.SynthProviders;
using SynthLib.Music;

namespace SynthLib.Board
{
    public class SuperBoard : ISynthProvider
    {
        private readonly ModuleBoard[] boards;

        private readonly BoardTemplate boardTemplate;

        private readonly Midi midi;

        public int SampleRate { get; }

        public bool Finished { get; private set; }

        public SuperBoard(BoardTemplate boardTemplate, Midi midi, int sampleRate = 44100)
        {
            SampleRate = sampleRate;
            Finished = false;
            boards = new ModuleBoard[128];
            this.boardTemplate = boardTemplate;
            this.midi = midi;
            midi.NoteOn += HandleNoteOn;
            midi.NoteOff += HandleNoteOff;
        }

        private void HandleNoteOn(int noteNumber)
        {
            boards[noteNumber] = boardTemplate.CreateInstance();
        }

        private void HandleNoteOff(int noteNumber)
        {
            boards[noteNumber] = null;
        }

        public float Next()
        {
            float result = 0;
            for (int i = 0; i < 128; ++i)
                result += boards[i] == null ? 0 : boards[i].Next((float)Tone.FrequencyFromNote(i));
            return result;
        }
    }
}
