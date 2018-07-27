using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using SynthLib.Board;
using SynthLib.Music;
using System.Diagnostics;
using NAudio.Midi;
using SynthLib.Data;

namespace SynthLib.MidiSampleProviders
{
    public class MonoBoard : IMidiSampleProvider
    {
        private readonly ModuleBoard board;

        private readonly BoardTemplate boardTemplate;

        public int SampleRate { get; }

        private readonly float glideSamples;

        private readonly float glideTime;

        private float destFreq;

        private float freqPerSample;

        private List<int> currentTones;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="boardTemplate"></param>
        /// <param name="glideTime">Glide time in milliseconds</param>
        /// <param name="sampleRate"></param>
        public MonoBoard(BoardTemplate boardTemplate, float glideTime, SynthData data)
        {
            SampleRate = data.SampleRate;
            this.boardTemplate = boardTemplate;
            board = boardTemplate.CreateInstance(data);
            this.glideTime = glideTime;
            glideSamples = (glideTime * SampleRate / 1000);
            currentTones = new List<int>();
        }

        public IMidiSampleProvider Clone(SynthData data)
        {
            return new MonoBoard(boardTemplate, glideTime, data);
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

        public void HandlePitchWheelChange(int pitch)
        {
            board.PitchWheelChange(pitch);
        }

        public void HandleControlChange(MidiController controller, int controllerValue)
        {
            board.ControllerChange(controller, controllerValue);
        }

        private void ChangeNote(int noteNumber)
        {
            Debug.Assert(On);
            destFreq = (float)Tone.FrequencyFromNote(noteNumber);
            freqPerSample = (destFreq - board.BaseFrequency) / glideSamples;

        }

        public void Next(float[] buffer, int offset, int count, float gain)
        {
            for (int i = offset; i < count + offset; i += 2)
            {
                board.BaseFrequency += freqPerSample;
                if (freqPerSample > 0 && board.BaseFrequency > destFreq || freqPerSample < 0 && board.BaseFrequency < destFreq)
                {
                    freqPerSample = 0;
                    board.BaseFrequency = destFreq;
                }
                (buffer[i], buffer[i + 1]) = board.Next();
            }
        }
    }
}
