using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Midi;

namespace SynthLib.MidiSampleProviders
{
    public interface IMidiSampleProvider
    {
        int SampleRate { get; }

        void HandleNoteOn(int noteNumber);

        void HandleNoteOff(int noteNumber);

        void HandleControlChange(MidiController controller, int controllerValue);

        IMidiSampleProvider Clone();

        void Next(float[] buffer, int offset, int count, float gain);
    }
}
