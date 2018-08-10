using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Midi;
using SynthLib.Data;

namespace SynthLib.MidiSampleProviders
{
    public class NullProvider : IMidiSampleProvider
    {
        public int SampleRate { get; }

        public IMidiSampleProvider Clone(SynthData data)
        {
            return new NullProvider();
        }

        public void HandleControlChange(MidiController controller, int controllerValue)
        {
            
        }

        public void HandleNoteOff(int noteNumber)
        {
            
        }

        public void HandleNoteOn(int noteNumber)
        {
            
        }

        public void HandlePitchWheelChange(int pitch)
        {
            
        }

        public void Next(float[] buffer, int offset, int count, float gain)
        {
            
        }
    }
}
