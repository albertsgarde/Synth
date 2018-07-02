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
        private readonly List<ModuleBoard> boards;

        private readonly ModuleBoard baseBoard;

        public int SampleRate { get; }

        public bool Finished { get; private set; }

        public SuperBoard(ModuleBoard baseBoard, Midi midi, int sampleRate = 44100)
        {
            SampleRate = sampleRate;
            Finished = false;
            boards = new List<ModuleBoard>();
            this.baseBoard = baseBoard.
        }

        private void HandleNoteOn(int noteNumber)
        {
            boards.Add()
        }

        public float Next()
        {
            throw new NotImplementedException();
        }
    }
}
