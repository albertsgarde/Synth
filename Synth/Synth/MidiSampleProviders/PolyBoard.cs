﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynthLib.Music;
using Stuff;
using NAudio.Wave;
using SynthLib.Board;

namespace SynthLib.MidiSampleProviders
{
    public class PolyBoard : IMidiSampleProvider
    {
        private readonly ModuleBoard[] boards;

        private readonly BoardTemplate boardTemplate;

        private readonly int voices;

        public int SampleRate { get; }

        public PolyBoard(BoardTemplate boardTemplate, int voices, int sampleRate = 44100)
        {
            SampleRate = sampleRate;
            this.boardTemplate = boardTemplate;
            this.voices = voices;
            boards = new ModuleBoard[voices];

            for (int i = 0; i < voices; ++i)
                boards[i] = boardTemplate.CreateInstance(sampleRate);
        }

        public void HandleNoteOn(int noteNumber)
        {
            if (boards.Count(mb => !mb.IsNoteOn) > 0)
                boards.Where(mb => !mb.IsNoteOn).MaxValue(mb => mb.Time).NoteOn(noteNumber);
            else
                boards.MaxValue(mb => mb.Time).NoteOn(noteNumber);
        }

        public void HandleNoteOff(int noteNumber)
        {
            foreach (var mb in boards)
            {
                if (mb.Note == noteNumber)
                    mb.NoteOff();
            }
        }

        public void HandleControlChange(int controllerValueValue)
        {

        }

        public IMidiSampleProvider Clone()
        {
            return new PolyBoard(boardTemplate, voices, SampleRate);
        }

        public void Next(float[] buffer, int offset, int count, float gain)
        {
            for (int i = offset; i < count + offset; ++i)
                buffer[i] = 0;
            Parallel.ForEach(boards, b =>
            {
                for (int i = offset; i < count + offset; i += 2)
                {
                    var (left, right) = b.Next();
                    lock (buffer)
                    {
                        buffer[i] += left;
                        buffer[i + 1] += right;
                    }
                }
            });

            for (int i = offset; i < count + offset; ++i)
            {
                buffer[i] *= gain;
            }
        }
    }
}
