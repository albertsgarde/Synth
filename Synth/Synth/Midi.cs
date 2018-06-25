using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Midi;

namespace SynthLib
{
    public class Midi
    {
        private readonly MidiIn midiIn;

        public int CurrentNoteNumber { get; private set; }

        public bool On { get; private set; }

        public Midi (MidiIn midiIn)
        {
            this.midiIn = midiIn;
            this.midiIn.MessageReceived += HandleMidiIn;
            this.midiIn.ErrorReceived += HandleMidiError;
            this.midiIn.Start();
            CurrentNoteNumber = 0;
            On = false;
        }

        private void HandleMidiIn(object sender, MidiInMessageEventArgs e)
        {
            if (MidiEvent.IsNoteOn(e.MidiEvent))
            {
                CurrentNoteNumber = ((NoteOnEvent)e.MidiEvent).NoteNumber;
                On = true;
            }
            else if (MidiEvent.IsNoteOff(e.MidiEvent))
            {
                if (((NoteEvent)e.MidiEvent).NoteNumber == CurrentNoteNumber)
                    On = false;
            }
        }

        private void HandleMidiError(object sender, MidiInMessageEventArgs e)
        {
            Console.WriteLine(e);
        }
    }
}
