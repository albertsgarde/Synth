using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynthLib.Music;
using SynthLib.Oscillators;

namespace SynthLib.SynthProviders
{
    public class MelodyPlayer : ISynthProvider
    {
        private readonly Melody melody;

        private readonly BPM bpm;

        private readonly IOscillator oscillator;

        private readonly IEnumerator<Note> melodyEnumerator;

        private double positionInNote;

        public int SampleRate { get; }

        public bool Finished { get; private set; }

        public MelodyPlayer(Melody melody, BPM bpm, IOscillator oscillator, int sampleRate = 44100)
        {
            this.melody = melody;
            this.bpm = bpm;
            this.oscillator = oscillator;
            melodyEnumerator = (IEnumerator<Note>)melody.GetEnumerator();
            positionInNote = 0;
            SampleRate = sampleRate;
            Finished = !melodyEnumerator.MoveNext();
        }

        public MelodyPlayer(Melody melody, BPM bpm, double duration, int sampleRate = 44100)
            : this(melody, bpm, new SineOscillator(sampleRate), sampleRate)
        {

        }

        public float[] Next(int samples)
        {
            throw new NotImplementedException();
            /*while (!Finished && positionInNote >= melodyEnumerator.Current.Duration(bpm))
            {
                positionInNote -= melodyEnumerator.Current.Duration(bpm);
                oscillator.Reset();
                if (Finished = !melodyEnumerator.MoveNext())
                    return 0;
            }
            positionInNote += 1d / SampleRate;
            return oscillator.NextValue((float)melodyEnumerator.Current.Tone.Frequency);*/
        }
    }
}
