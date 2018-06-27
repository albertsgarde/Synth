﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynthLib.Oscillators;
using SynthLib.Music;
using SynthLib.ValueProviders;

namespace SynthLib.Board.Modules
{
    public class OscillatorModule : Module
    {
        private readonly IOscillator oscillator;

        public ValueReciever Gain { get; }

        private readonly float frequencyMultiplier;

        private readonly Midi midi;

        public override Connections Inputs { get; }

        public override Connections Outputs { get; }

        public override string Type { get; } = "Oscillator";

        public OscillatorModule(IOscillator oscillator, Midi midi, int outputs, float halfToneOffset = 0, float gain = 1f)
        {
            this.oscillator = oscillator.Clone();
            frequencyMultiplier = (float) Math.Pow(2, (1 / 12d) * halfToneOffset);
            Gain = new ValueReciever(new Constant(gain), 0, 1);
            Inputs = new ConnectionsArray(0);
            Outputs = new ConnectionsArray(outputs);
            this.midi = midi;
        }

        public override void Next()
        {
            var frequency = Tone.FrequencyFromNote(midi.CurrentNoteNumber);
            oscillator.Next(frequency * frequencyMultiplier);
        }

        public override float[] Process(float[] inputs)
        {
            var output = new float[Outputs.Count];
            if (!midi.On)
                return output;
            var next = oscillator.CurrentValue();
            for (int i = 0; i < output.Length; ++i)
                output[i] = next * Gain.CurrentValue();
            return output;
        }
    }
}