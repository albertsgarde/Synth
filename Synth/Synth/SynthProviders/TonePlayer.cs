using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynthLib.Music;
using SynthLib.Oscillators;

namespace SynthLib.SynthProviders
{
    public class TonePlayer : ISynthProvider
    {
        private readonly double frequency;

        private readonly IOscillator oscillator;

        private readonly long duration;

        private long position;

        public int SampleRate { get; }

        public bool Finished { get; private set; }

        public TonePlayer(double frequency, double duration, IOscillator oscillator, int sampleRate = 44100)
        {
            this.frequency = frequency;
            this.oscillator = oscillator;
            this.duration = (long)(duration * sampleRate);
            position = 0;
            SampleRate = sampleRate;
            Finished = false;
        }

        public TonePlayer(double frequency, double duration, int sampleRate = 44100) 
            : this(frequency, duration, new SineOscillator(sampleRate), sampleRate)
        {
        }

        public TonePlayer(Tone tone, double duration, IOscillator oscillator, int sampleRate = 44100)
        {
            frequency = tone.Frequency;
            this.oscillator = oscillator;
            this.duration = (long)(duration * sampleRate);
            position = 0;
            SampleRate = sampleRate;
            Finished = false;
        }

        public TonePlayer(Tone tone, double duration, int sampleRate = 44100) 
            : this(tone, duration, new SineOscillator(sampleRate), sampleRate)
        {
        }

        public float Next()
        {
            if (position++ >= duration)
                Finished = true;
            return oscillator.Next(frequency);
        }
    }
}
