using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynthLib.Oscillators;
using SynthLib.Music;
using NAudio.Midi;
using SynthLib.Board;
using NAudio.Wave;

namespace SynthLib
{
    public static class SynthUtils
    {
        public static void PlayMidiToFile(string midiFile, string destFile, IMidiSampleProvider msp)
        {
            float sampleNum = 0;

            var file = new MidiFile(midiFile);
            int ticksPerQuarterNote = file.DeltaTicksPerQuarterNote;
            var events = file.Events.Aggregate((IEnumerable<MidiEvent>)new List<MidiEvent>(), (totalList, list) => totalList.Concat(list)).OrderBy(me => me.AbsoluteTime);
            var totalEvents = events.Count();

            var board = msp.Clone();
            var midi = new Midi(ticksPerQuarterNote);
            midi.NoteOn += board.HandleNoteOn;
            midi.NoteOff += board.HandleNoteOff;

            using (var output = new WaveFileWriter(destFile, board.WaveFormat))
            {
                int eventCount = 0;
                foreach (var me in events)
                {
                    var absSample = midi.MidiTicksToSamples(me.AbsoluteTime, board.SampleRate);
                    var samples = (int)(absSample - sampleNum);

                    var result = new float[samples];
                    board.Read(result, 0, samples);

                    output.WriteSamples(result, 0, samples);
                    sampleNum = absSample;
                    midi.HandleMidiEvent(me);
                    Console.WriteLine($"{++eventCount}/{totalEvents}");
                }
            }
        }

        public static void PlayMidi(string midiFile, IMidiSampleProvider msp, int desiredLatency)
        {
            //throw new Exception("Doesn't work yet");
            var file = new MidiFile(midiFile);
            int ticksPerQuarterNote = file.DeltaTicksPerQuarterNote;
            var events = file.Events.Aggregate((IEnumerable<MidiEvent>)new List<MidiEvent>(), (totalList, list) => totalList.Concat(list)).OrderBy(me => me.AbsoluteTime);
            var totalEvents = events.Count();

            var board = msp.Clone();
            var midi = new Midi(ticksPerQuarterNote);
            midi.NoteOn += board.HandleNoteOn;
            midi.NoteOff += board.HandleNoteOff;

            var aOut = new WaveOutEvent
            {
                DesiredLatency = desiredLatency,
                DeviceNumber = -1
            };
            aOut.Init(board);
            aOut.Play();

            long startTime = DateTime.Now.Ticks;
            foreach (var me in events)
            {
                while (DateTime.Now.Ticks - startTime < midi.MidiTicksToDateTimeTicks(me.AbsoluteTime)) ;
                midi.HandleMidiEvent(me);

            }
        }

        public static float[] Next(this ISampleProvider sp, int samples)
        {
            var result = new float[samples];
            sp.Read(result, 0, samples);
            return result;
        }
    }
}
