using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace SynthLib.MidiSampleProviders
{
    public interface IMidiSampleProvider : ISampleProvider
    {
        int SampleRate { get; }

        void HandleNoteOn(int noteNumber);

        void HandleNoteOff(int noteNumber);

        IMidiSampleProvider Clone();
    }
}
