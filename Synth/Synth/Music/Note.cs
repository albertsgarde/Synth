using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Stuff;

namespace SynthLib.Music
{
    public class Note
    {
        // Note values
        public const double DOUBLE_WHOLE_NOTE = 8;

        public const double WHOLE_NOTE = 4;

        public const double HALF_NOTE = 2;

        public const double QUARTER_NOTE = 1;

        public const double EIGHTH_NOTE = 0.5;

        public const double SIXTEENTH_NOTE = 0.25;

        public Tone Tone { get; }
        
        /// <summary>
        /// The duration of the note as fraction of a whole note (4 fourths).
        /// </summary>
        public double Value { get; }

        public Note(Tone tone, double value)
        {
            Tone = tone;
            Value = value;
        }

        public Note(XElement element, IReadOnlyDictionary<string, double> noteValues)
        {
            if (element.Contains("tone"))
                Tone = new Tone(element.ElementValue("tone"));
            else
                Tone = null;
            if (element.Element("value").HasElements)
            {
                Value = 0;
                foreach (var subValue in element.Element("value").Elements())
                {
                    Value += LoadNoteValue(subValue.Value, noteValues);
                }
            }
            else
                Value = LoadNoteValue(element.ElementValue("value"), noteValues);
        }

        private static double LoadNoteValue(string value, IReadOnlyDictionary<string, double> noteValues)
        {
            double result;
            if (noteValues.ContainsKey(value))
                result = noteValues[value];
            else if (!double.TryParse(value, out result))
                throw new ArgumentException("Failed to load note value. Value neither a number or a predefined note value. value: " + value);
            return result;
        }

        public double Duration(BPM bpm)
        {
            return Value * 60 / bpm.QuarterNotesPerMinute;
        }

        public void Beep(BPM bpm)
        {
            if (Tone != null)
            {
                Console.Beep((int)Tone.Frequency, (int)(Value * 60000 / bpm.QuarterNotesPerMinute));
            }
        }

        public override string ToString()
        {
            return "{Tone: " + Tone + " Value: " + Value + "}";
        }
    }
}
