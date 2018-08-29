using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Midi;
using Stuff;
using System.Diagnostics;

namespace SynthLib
{
    public class Midi
    {
        private MidiIn midiIn;

        private readonly List<int> currentNoteNumbers;

        public IReadOnlyCollection<int> CurrentNoteNumbers { get; private set; }

        public delegate void ControlChangeEventHandler(MidiController controller, int controllerValue);

        public event ControlChangeEventHandler ControlChange;

        public delegate void PitchWheelChangeEventHandler(int pitch);

        public event PitchWheelChangeEventHandler PitchWheelChange;

        public delegate void NoteEventHandler(int noteNumber);

        public event NoteEventHandler NoteOn;

        public event NoteEventHandler NoteOff;

        private int microsecondsPerQuarterNote;

        private readonly int ticksPerQuarterNote;

        public static IReadOnlyList<float> Frequencies { get; }

        static Midi()
        {
            var frequencies = new List<float>(128);
            for (int i = 0; i < 128; ++i)
                frequencies.Add((float)Music.Tone.FrequencyFromNote(i));
            Frequencies = frequencies;
        }

        public Midi(int ticksPerQuarterNote)
        {
            this.ticksPerQuarterNote = ticksPerQuarterNote;
            currentNoteNumbers = new List<int>();
            CurrentNoteNumbers = currentNoteNumbers;
            midiIn = null;
            microsecondsPerQuarterNote = 2000000; // 120 Bpm = 2 Bps
        }

        public void SetMidiIn(int deviceNo)
        {
            try
            {
                midiIn = new MidiIn(deviceNo);
            }
            catch (NAudio.MmException mme)
            {
                Console.WriteLine(MidiIn.NumberOfDevices);
                Console.WriteLine(MidiIn.DeviceInfo(deviceNo).ProductName);
                Console.WriteLine($"Midi device {deviceNo} not available.");
                throw mme;
            }
            midiIn.MessageReceived += HandleMidiIn;
            midiIn.ErrorReceived += HandleMidiError;
            midiIn.Start();
        }

        private void HandleNoteOn(int noteNumber)
        {
            currentNoteNumbers.Add(noteNumber);
            NoteOn?.Invoke(noteNumber);
        }

        private void HandleNoteOff(int noteNumber)
        {
            currentNoteNumbers.Remove(noteNumber);
            NoteOff?.Invoke(noteNumber);
        }

        private void HandlePitchWheelChange(int pitch)
        {
            PitchWheelChange?.Invoke(pitch);
        }

        private void HandleControlChange(MidiController controller, int controllerValue)
        {
            ControlChange?.Invoke(controller, controllerValue);
        }

        private void HandleTempoChange(int microsecondsPerQuarterNote)
        {
            this.microsecondsPerQuarterNote = microsecondsPerQuarterNote;
        }

        public void HandleMidiEvent(MidiEvent me)
        {
            if (MidiEvent.IsNoteOn(me))
                HandleNoteOn(((NoteOnEvent)me).NoteNumber);
            else if (MidiEvent.IsNoteOff(me))
                HandleNoteOff(((NoteEvent)me).NoteNumber);
            else if (me.CommandCode == MidiCommandCode.PitchWheelChange)
                HandlePitchWheelChange(((PitchWheelChangeEvent)me).Pitch);
            else if (me.CommandCode == MidiCommandCode.ControlChange)
                HandleControlChange(((ControlChangeEvent)me).Controller, ((ControlChangeEvent)me).ControllerValue);
            else if (me.CommandCode == MidiCommandCode.MetaEvent)
            {
                Debug.Assert(me is MetaEvent);
                if (me is TempoEvent)
                    HandleTempoChange(((TempoEvent)me).MicrosecondsPerQuarterNote);
            }
        }

        private void HandleMidiIn(object sender, MidiInMessageEventArgs e)
        {
            HandleMidiEvent(e.MidiEvent);
        }

        public long MidiTicksToDateTimeTicks(long midiTicks)
        {
            return midiTicks * (microsecondsPerQuarterNote * TimeSpan.TicksPerMillisecond / 1000) / ticksPerQuarterNote;
        }

        public float MidiTicksToSamples(long midiTicks, int sampleRate = 44100)
        {
            return midiTicks * ((float)sampleRate * microsecondsPerQuarterNote) / ticksPerQuarterNote / 1000000;
        }

        private void HandleMidiError(object sender, MidiInMessageEventArgs e)
        {
            Console.WriteLine(e);
        }
    }
}
