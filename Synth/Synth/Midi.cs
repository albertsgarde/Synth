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

        private List<int> currentNoteNumbers;

        public IReadOnlyCollection<int> CurrentNoteNumbers { get; private set; }

        public delegate void NoteEventHandler(int noteNumber);

        public event NoteEventHandler NoteOn;

        public event NoteEventHandler NoteOff;

        public static IReadOnlyList<float> Frequencies { get; }

        static Midi()
        {
            var frequencies = new List<float>(128);
            for (int i = 0; i < 128; ++i)
                frequencies.Add((float)Music.Tone.FrequencyFromNote(i));
            Frequencies = frequencies;
        }

        public Midi (MidiIn midiIn)
        {
            currentNoteNumbers = new List<int>();
            CurrentNoteNumbers = currentNoteNumbers;
            this.midiIn = midiIn;
            this.midiIn.MessageReceived += HandleMidiIn;
            this.midiIn.ErrorReceived += HandleMidiError;
            this.midiIn.Start();
        }

        private void HandleMidiIn(object sender, MidiInMessageEventArgs e)
        {
            if (MidiEvent.IsNoteOn(e.MidiEvent))
            {
                var noteNumber = ((NoteOnEvent)e.MidiEvent).NoteNumber;
                currentNoteNumbers.Add(noteNumber);
                NoteOn?.Invoke(noteNumber);
            }
            else if (MidiEvent.IsNoteOff(e.MidiEvent))
            {
                var noteNumber = ((NoteEvent)e.MidiEvent).NoteNumber;
                currentNoteNumbers.Remove(noteNumber);
                NoteOff?.Invoke(noteNumber);
            }
        }

        private void HandleMidiError(object sender, MidiInMessageEventArgs e)
        {
            Console.WriteLine(e);
        }
    }
}
