﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff;

namespace SynthLib.Music
{
    public class Tone
    {
        public static readonly Tone CONCERT_A = new Tone(6, 4);

        public static readonly char[] Letters = { 'C', 'D', 'E', 'F', 'G', 'A', 'B' };

        /// <summary>
        /// The number of the interval from the C below it to this note.
        /// </summary>
        public int Letter { get; private set; }

        public int Octave { get; private set; }

        public int Modifier { get; private set; }

        /// <summary>
        /// The number of half-tones this note is from C0.
        /// </summary>
        public int ToneNumber { get; private set; }

        private double? frequency = null;
        

        public Tone(int letter, int octave, int modifier = 0)
        {
            Letter = letter;
            Octave = octave;
            Modifier = modifier;
            ToneNumber = (Octave + 1) * 12 + Interval.INTERVAL_LENGTHS[Letter - 1] + Modifier;
        }

        public Tone(int noteNumber)
        {
            ToneNumber = noteNumber;
            Octave = 0;

            noteNumber -= 12; // Midi numbers start at C-1
            if (noteNumber < 0)
            {
                while (noteNumber < 0)
                {
                    noteNumber += 12;
                    Octave--;
                }
            }
            else if (noteNumber >= 12)
            {
                while (noteNumber >= 12)
                {
                    noteNumber -= 12;
                    Octave++;
                }
            }
            
            Letter = Interval.HalfTonesToNumber(noteNumber);
            Modifier = noteNumber - Interval.INTERVAL_LENGTHS[Letter - 1];

        }

        private Tone(Tone tone)
        {
            Letter = tone.Letter;
            Octave = tone.Octave;
            Modifier = tone.Modifier;
            ToneNumber = tone.ToneNumber;
        }

        public Tone(string name)
        {
            try
            {
                Letter = Letters.IndexOf(name[0]) + 1;
            }
            catch
            {
                throw new Exception("Invalid tone letter. Does not exist.");
            }
            Modifier = 0;
            int i = 1;
            if (name[i] == '#')
            {
                for (; name[i] == '#'; i++)
                    Modifier++;
            }
            else if (name[i] == 'b')
            {
                for (; name[i] == 'b'; i++)
                    Modifier--;
            }
            if (!int.TryParse(name.Substring(i), out int octave))
                throw new Exception("Could not read tone. Incorrect format.");
            Octave = octave;
            ToneNumber = (octave + 1) * 12 + Interval.NumberToHalfTones(Letter) + Modifier;
        }

        public double Frequency
        {
            get
            {
                //return frequency ?? (double)(frequency = (440D / Math.Pow(2, (1 / 12D) * HalfToneDiff(CONCERT_A))));
                return frequency ?? (double)(frequency = FrequencyFromNote(ToneNumber));
            }
        }

        public static double FrequencyFromNote(int noteNumber)
        {
            return (440D / Math.Pow(2, (1 / 12D) * (69 - noteNumber)));
        }

        public static string ModifierToString(int modifier)
        {
            string result = "";
            if (modifier > 0)
            {
                for (; modifier > 0; modifier--)
                    result += "#";
            }
            else if (modifier < 0)
            {
                for (; modifier < 0; modifier++)
                    result += "b";
            }
            return result;
        }

        public static Tone operator +(Tone note, Interval interval)
        {
            return new Tone(note.ToneNumber + interval.HalfTones);
        }

        public static Tone operator -(Tone note, Interval interval)
        {
            return new Tone(note.ToneNumber - interval.HalfTones);
        }

        public static Interval operator -(Tone note1, Tone note2)
        {
            return new Interval(note1.ToneNumber - note2.ToneNumber);
        }

        public int HalfToneDiff(Tone note)
        {
            return note.ToneNumber - ToneNumber;
        }

        public Tone AddModifier(int modifier)
        {
            var result = new Tone(this);
            result.Modifier += modifier;
            return result;
        }
        
        /// <returns>This note in the specified octave.</returns>
        public Tone ToOctave(int octave)
        {
            return this + (octave - Octave) * 12;
        }

        public void Beep(int length)
        {
            Console.WriteLine(this);
            Console.Beep((int)Frequency, length);
        }

        public bool IsEqual(Tone note)
        {
            return ToneNumber == note.ToneNumber;
        }

        public override string ToString()
        {
            return Letters[Letter - 1] + ModifierToString(Modifier) + Octave;
        }
    }
}