using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynthLib.Oscillators;
using SynthLib.Music;

namespace SynthLib.Board.Modules
{
    public class OscillatorModule : Module
    {
        private readonly IOscillator oscillator;

        private readonly float gain;

        private readonly float frequencyMultiplier;

        private readonly Midi midi;

        public override Connections Inputs { get; }

        public override Connections Outputs { get; }

        public override string Type { get; } = "Oscillator";

        public OscillatorModule(IOscillator oscillator, Midi midi, int outputs, float halfToneOffset = 0, float gain = 1)
        {
            this.oscillator = oscillator.Clone();
            frequencyMultiplier = (float) Math.Pow(2, (1 / 12d) * halfToneOffset);
            this.gain = gain;
            Inputs = new ConnectionsArray(0);
            Outputs = new ConnectionsArray(outputs);
            this.midi = midi;
        }

        public override float[] Process(float[] inputs)
        {
            var output = new float[Outputs.Count];
            if (!midi.On)
                return output;
            var frequency = Tone.FrequencyFromNote(midi.CurrentNoteNumber);
            var next = oscillator.NextValue(frequency * frequencyMultiplier);
            for (int i = 0; i < output.Length; ++i)
                output[i] = next;
            return output;
        }
    }
}
