using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynthLib.Oscillators;
using SynthLib.SynthProviders;
using SynthLib.Music;

namespace SynthLib
{
    public static class SynthUtils
    {
        public static void PlayTone(this SynthResult sr, double frequency, double duration, IOscillator oscillator)
        {
            var tonePlayer = new TonePlayer(frequency, duration, oscillator, sr.WaveFormat.SampleRate);
            sr.AddSynthProvider(tonePlayer);
        }
        public static void PlayTone(this SynthResult sr, double frequency, double duration)
        {
            sr.PlayTone(frequency, duration, new SineOscillator(sr.WaveFormat.SampleRate));
        }

        public static void PlayTone(this SynthResult sr, Tone tone, double duration, IOscillator oscillator)
        {
            var tonePlayer = new TonePlayer(tone, duration, oscillator, sr.WaveFormat.SampleRate);
            sr.AddSynthProvider(tonePlayer);
        }
        public static void PlayTone(this SynthResult sr, Tone tone, double duration)
        {
            sr.PlayTone(tone, duration, new SineOscillator(sr.WaveFormat.SampleRate));
        }

        public static void PlayChord(this SynthResult sr, Chord chord, int octave, double duration, IOscillator oscillator)
        {
            foreach (var tone in chord.Tones(octave))
            {
                var tonePlayer = new TonePlayer(tone, duration, oscillator, sr.WaveFormat.SampleRate);
                sr.AddSynthProvider(tonePlayer);
            }
        }

        public static void PlayChord(this SynthResult sr, Chord chord, int octave, double duration)
        {
            sr.PlayChord(chord, octave, duration, new SineOscillator(sr.WaveFormat.SampleRate));
        }

        public static void PlayMelody(this SynthResult sr, Melody melody, BPM bpm, IOscillator oscillator)
        {
            var melodyPlayer = new MelodyPlayer(melody, bpm, oscillator, sr.WaveFormat.SampleRate);
            sr.AddSynthProvider(melodyPlayer);
        }

        public static void PlayMelody(this SynthResult sr, Melody melody, BPM bpm)
        {
            sr.PlayMelody(melody, bpm, new SineOscillator(sr.WaveFormat.SampleRate));
        }
    }
}
