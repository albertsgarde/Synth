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

        public WaveFormat WaveFormat { get; }

        private readonly IMidiSampleProvider lowerBoard;

        private readonly IMidiSampleProvider upperBoard;

        private readonly int splitNoteNumber;

        public SplitBoard(IMidiSampleProvider lowerBoard, IMidiSampleProvider upperBoard, int splitNoteNumber)
        {
            Console.WriteLine("Should do something with waveformats");
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

        public int Read(float[] buffer, int offset, int count)
        {
            var lowerResult = new float[buffer.Length];
            var upperResult = new float[buffer.Length];
            lowerBoard.Read(lowerResult, offset, count);
            upperBoard.Read(upperResult, offset, count);
            for (int i = offset; i < count; ++i)
                buffer[i] = lowerResult[i] + upperResult[i];
            return count;
        }
    }
}
