using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Midi;
using NAudio.Wave;
using SynthLib.Data;

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

        public SplitBoard(IMidiSampleProvider lowerBoard, IMidiSampleProvider upperBoard, int splitNoteNumber, SynthData data)
        {
            this.lowerBoard = lowerBoard.Clone(data);
            this.upperBoard = upperBoard.Clone(data);
            this.splitNoteNumber = splitNoteNumber;
        }

        public IMidiSampleProvider Clone(SynthData data)
        {
            return new SplitBoard(lowerBoard.Clone(data), upperBoard.Clone(data), splitNoteNumber, data);
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

        public void HandlePitchWheelChange(int pitch)
        {
            lowerBoard.HandlePitchWheelChange(pitch);
            upperBoard.HandlePitchWheelChange(pitch);
        }

        public void HandleControlChange(MidiController controller, int controllerValue)
        {
            lowerBoard.HandleControlChange(controller, controllerValue);
            upperBoard.HandleControlChange(controller, controllerValue);
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
