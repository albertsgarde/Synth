using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Midi;
using SynthLib.Data;

namespace SynthLib.MidiSampleProviders
{
    public interface IMidiSampleProvider
    {
        int SampleRate { get; }

        (float left, float right) MaxValue { get; }

        void HandleNoteOn(int noteNumber);

        void HandleNoteOff(int noteNumber);

        void HandlePitchWheelChange(int pitch);

        void HandleControlChange(MidiController controller, int controllerValue);

        IMidiSampleProvider Clone(SynthData data);

        void Next(float[] buffer, int offset, int count, float gain);
    }
}
