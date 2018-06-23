using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynthLib.Oscillators;
using NAudio.Midi;
using SynthLib.Music;

namespace SynthLib.SynthProviders
{
    public class KeyboardPlayer : ISynthProvider
    {
        private readonly IOscillator oscillator;

        private readonly Key[] keys;

        private readonly MidiIn midiIn;

        public int SampleRate { get; }

        public bool Finished { get; private set; }

        public KeyboardPlayer(MidiIn midiIn, IOscillator oscillator, int sampleRate = 44100)
        {
            this.oscillator = oscillator;
            keys = new Key[127];
            for (int i = 0; i < keys.Length; ++i)
                keys[i] = new Key(oscillator.Clone(), i);
            this.midiIn = midiIn;
            this.midiIn.MessageReceived += HandleMidiIn;
            this.midiIn.ErrorReceived += HandleMidiError;
            SampleRate = sampleRate;
            Finished = false;
            this.midiIn.Start();
        }

        private void HandleMidiIn(object sender, MidiInMessageEventArgs e)
        {
            if (MidiEvent.IsNoteOn(e.MidiEvent))
            {
                var noteNumber = ((NoteOnEvent)e.MidiEvent).NoteNumber;
                keys[noteNumber].Activate();
            }
            else if (MidiEvent.IsNoteOff(e.MidiEvent))
            {
                var noteNumber = ((NoteEvent)e.MidiEvent).NoteNumber;
                keys[noteNumber].Deactivate();
            }
        }

        private void HandleMidiError(object sender, MidiInMessageEventArgs e)
        {
            Console.WriteLine(e);
        }

        private class Key
        {
            private IOscillator osc;

            private double frequency;

            private bool active;

            public Key(IOscillator oscillator, int noteNumber)
            {
                osc = oscillator;
                frequency = Tone.FrequencyFromNote(noteNumber);
                active = false;
            }

            public void Activate()
            {
                active = true;
            }

            public void Deactivate()
            {
                active = false;
            }

            public float Next()
            {
                return active ? osc.NextValue(frequency) : 0;
            }

            // TODO: End() and fade.
        }

        public float Next()
        {
            return keys.Aggregate(0f, (total, key) => total + key.Next());
        }
    }
}
