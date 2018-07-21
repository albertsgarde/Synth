using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Music
{
    /// <summary>
    /// Does not yet have full support for augmented and diminished intervals.
    /// </summary>
    public class Interval
    {
        public enum IntervalQuality { Diminished, Minor, Perfect, Major, Augmented }

        public static readonly int[] INTERVAL_LENGTHS = { 0, 2, 4, 5, 7, 9, 11 };

        private static readonly string[] intervalNames = { "Prime", "Second", "Third", "Fourth", "Fifth", "Sixth", "Seventh", "Octave" };

        public int Number { get; }
        
        public bool Direction { get; }

        public IntervalQuality Quality { get; }

        private readonly int noteNumQualitySize;

        public int HalfTones { get; }

        public Interval(int number, IntervalQuality quality)
        {
            Direction = number >= 0;
            Number = number;

            Quality = quality;
            noteNumQualitySize = QualitySize(quality, number);
            HalfTones = 0;

            number = Math.Abs(number);
            while (number > 7)
            {
                HalfTones += 12;
                number -= 7;
            }
            HalfTones += INTERVAL_LENGTHS[number - 1];
            HalfTones += noteNumQualitySize;
            if (!Direction)
                HalfTones *= -1;
        }

        public Interval(int halfTones)
        {
            Direction = halfTones >= 0;
            HalfTones = halfTones;
            halfTones = Math.Abs(halfTones);

            Number = HalfTonesToNumber(halfTones) * (Direction ? 1 : -1);

            int number = (Math.Abs(Number) - 1) % 7 + 1;
            switch (number)
            {
                case 1:
                case 4:
                case 5:
                    Quality = IntervalQuality.Perfect;
                    break;
                case 2:
                case 3:
                case 6:
                case 7:
                    if (INTERVAL_LENGTHS[number - 1] > halfTones % 12)
                        Quality = IntervalQuality.Minor;
                    else
                        Quality = IntervalQuality.Major;
                    Debug.Assert(INTERVAL_LENGTHS[number - 1] >= halfTones % 12);
                    break;
                default:
                    throw new Exception("Should not be possible");
            }

        }

        public Interval(string s)
        {
            if (s.StartsWith("-"))
            {
                Direction = false;
                s = s.Substring(1);
            }
            else
                Direction = true;
            Quality = StringToQuality(s[0]);
            Number = int.Parse(s.Substring(1)) * (Direction ? 1 : -1);
            noteNumQualitySize = QualitySize(Quality, Number);
            HalfTones = NumberToHalfTones(Number) + noteNumQualitySize;
        }

        private static int QualitySize(IntervalQuality quality, int number)
        {
            switch ((Math.Abs(number)-1) % 7 + 1)
            {
                case 1:
                case 4:
                case 5:
                    switch (quality)
                    {
                        case IntervalQuality.Diminished:
                            return -1;
                        case IntervalQuality.Minor:
                            throw new InvalidEnumArgumentException("Invalid quality for interval. Quality: " + quality + " Interval: " + number);
                        case IntervalQuality.Perfect:
                            return 0;
                        case IntervalQuality.Major:
                            throw new InvalidEnumArgumentException("Invalid quality for interval. Quality: " + quality + " Interval: " + number);
                        case IntervalQuality.Augmented:
                            return 1;
                        default:
                            throw new Exception("Should not be possible");
                    }
                case 2:
                case 3:
                case 6:
                case 7:
                    switch (quality)
                    {
                        case IntervalQuality.Diminished:
                            return -2;
                        case IntervalQuality.Minor:
                            return -1;
                        case IntervalQuality.Perfect:
                            throw new InvalidEnumArgumentException("Invalid quality for interval. Quality: " + quality + " Interval: " + number);
                        case IntervalQuality.Major:
                            return 0;
                        case IntervalQuality.Augmented:
                            return 1;
                        default:
                            throw new Exception("Should not be possible");
                    }
                default:
                    throw new Exception("Should not be possible");

            }
        }

        private static string QualityToString(IntervalQuality quality)
        {
            switch (quality)
            {
                case IntervalQuality.Diminished:
                    return "d";
                case IntervalQuality.Minor:
                    return "m";
                case IntervalQuality.Perfect:
                    return "P";
                case IntervalQuality.Major:
                    return "M";
                case IntervalQuality.Augmented:
                    return "A";
                default:
                    throw new Exception("Should not be possible.");
            }
        }
        
        private static IntervalQuality StringToQuality(char s)
        {
            switch(s)
            {
                case 'd':
                    return IntervalQuality.Diminished;
                case 'm':
                    return IntervalQuality.Minor;
                case 'P':
                    return IntervalQuality.Perfect;
                case 'M':
                    return IntervalQuality.Major;
                case 'A':
                    return IntervalQuality.Augmented;
                default:
                    throw new Exception("Should not be possible.");
            }
        }

        public static int HalfTonesToNumber(int halfTones)
        {
            int result = 0;
            while (halfTones >= 12)
            {
                result += 7;
                halfTones -= 12;
            }
            int i = -1;
            while (INTERVAL_LENGTHS[++i] < halfTones);
            return result + i + 1;
        }

        public static int NumberToHalfTones(int number)
        {
            int result = 0;
            while (number > 7)
            {
                result += 12;
                number -= 7;
            }
            return result + INTERVAL_LENGTHS[number - 1];
        }

        public static Interval operator +(Interval intA, Interval intB)
        {
            return new Interval(intA.HalfTones + intB.HalfTones);
        }

        public static Interval operator -(Interval intA, Interval intB)
        {
            return new Interval(intA.HalfTones - intB.HalfTones);
        }

        public string ProperName()
        {
            int tempNumber = Number;
            while (tempNumber > 8)
                tempNumber -= 7;
            return Quality + " " + intervalNames[Math.Abs(tempNumber - 1)];
        }

        public static implicit operator Interval(int halftones)
        {
            return new Interval(halftones);
        }

        public override string ToString()
        {
            return (Direction ? "" : "-") + QualityToString(Quality) + Math.Abs(Number);
        }
    }
}
