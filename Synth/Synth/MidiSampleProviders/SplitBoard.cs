using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace SynthLib.MidiSampleProviders
{
    public class SplitBoard : IMidiSampleProvider
    {
        public int SampleRate { get; }

        private readonly IMidiSampleProvider lowerBoard;

        private readonly IMidiSampleProvider upperBoard;

        private readonly int splitNoteNumber;

        private float[] lowerResult;

        private float[] upperResult;

        public SplitBoard(IMidiSampleProvider lowerBoard, IMidiSampleProvider upperBoard, int splitNoteNumber)
        {
            this.lowerBoard = lowerBoard.Clone();
            this.upperBoard = upperBoard.Clone();
            this.splitNoteNumber = splitNoteNumber;
        }

        public IMidiSampleProvider Clone()
        {
            return new SplitBoard(lowerBoard.Clone(), upperBoard.Clone(), splitNoteNumber);
        }

        public void HandleNoteOn(int noteNumber)
        {
            if (noteNumber < splitNoteNumber)
                lowerBoard.HandleNoteOn(noteNumber);
            else
                upperBoard.HandleNoteOn(noteNumber);
        }

        public void HandleNoteOff(int noteNumber)
        {
            if (noteNumber < splitNoteNumber)
                lowerBoard.HandleNoteOff(noteNumber);
            else
                upperBoard.HandleNoteOff(noteNumber);
        }

        public void HandleControlChange(int controllerValue)
        {
            lowerBoard.HandleControlChange(controllerValue);
            upperBoard.HandleControlChange(controllerValue);
        }

        public void Next(float[] buffer, int offset, int count, float gain)
        {
            lowerResult = new float[buffer.Length];
            upperResult = new float[buffer.Length];
            lowerBoard.Next(lowerResult, offset, count, gain);
            upperBoard.Next(upperResult, offset, count, gain);
            for (int i = offset; i < offset + count; ++i)
            {
                buffer[i] = lowerResult[i] + upperResult[i];
            }
        }
    }
}
