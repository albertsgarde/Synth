using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using SynthLib.Board;
using SynthLib.Music;
using System.Diagnostics;

namespace SynthLib.MidiSampleProviders
{
    public class MonoBoard : IMidiSampleProvider
    {
        private readonly ModuleBoard board;

        private readonly BoardTemplate boardTemplate;

        public int SampleRate { get; }

        public WaveFormat WaveFormat { get; }

        private readonly int channel;

        private readonly float glideSamples;

        private readonly float glideTime;

        private float destFreq;

        private float freqPerSample;

        private List<int> currentTones;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="boardTemplate"></param>
        /// <param name="channel"></param>
        /// <param name="glideTime">Glide time in milliseconds</param>
        /// <param name="sampleRate"></param>
        public MonoBoard(BoardTemplate boardTemplate, int channel, float glideTime, int sampleRate = 44100)
        {
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, 16);
            SampleRate = sampleRate;
            this.channel = channel;
            this.boardTemplate = boardTemplate;
            board = boardTemplate.CreateInstance(sampleRate);
            this.glideTime = glideTime;
            glideSamples = (glideTime * SampleRate / 1000);
            currentTones = new List<int>();
        }

        public IMidiSampleProvider Clone()
        {
            return new MonoBoard(boardTemplate, channel, glideTime, SampleRate);
        }

        private bool On => currentTones.Count != 0;

        public void HandleNoteOn(int noteNumber)
        {
            if (On)
            {
                if (currentTones.Contains(noteNumber))
                    currentTones.Remove(noteNumber);
                currentTones.Add(noteNumber);
                ChangeNote(noteNumber);
            }
            else
            {
                board.NoteOn(noteNumber);
                currentTones.Add(noteNumber);
                freqPerSample = 0;
            }
        }

        public void HandleNoteOff(int noteNumber)
        {
            currentTones.Remove(noteNumber);
            if (On)
                ChangeNote(currentTones.Last());
            else
                board.NoteOff();
        }

        private void ChangeNote(int noteNumber)
        {
            Debug.Assert(On);
            destFreq = (float)Tone.FrequencyFromNote(noteNumber);
            freqPerSample = (destFreq - board.Frequency) / glideSamples;

        }

        public int Read(float[] buffer, int offset, int count)
        {
            for (int i = offset; i < count; ++i)
            {
                board.Frequency += freqPerSample;
                if (freqPerSample > 0 && board.Frequency > destFreq || freqPerSample < 0 && board.Frequency < destFreq)
                {
                    freqPerSample = 0;
                    board.Frequency = destFreq;
                }
                buffer[i] = board.Next();
            }
            return count;
        }
    }
}
