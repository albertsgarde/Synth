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
        public static readonly Dictionary<string, double> NoteValues = LoadNoteValues();

        // Note values
        public const double DOUBLE_WHOLE_NOTE = 2;

        public const double WHOLE_NOTE = 1;

        public const double HALF_NOTE = 0.5;

        public const double QUARTER_NOTE = 0.25;

        public const double EIGHTH_NOTE = 0.125;

        public const double SIXTEENTH_NOTE = 0.0625;

        public Tone Tone { get; private set; }
        
        /// <summary>
        /// The duration of the note as fraction of a whole note (4 fourths).
        /// </summary>
        public double Value { get; private set; }

        public Note(Tone tone, double value)
        {
            Tone = tone;
            Value = value;
        }

        public Note(XElement element)
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
                    Value += LoadNoteValue(subValue.Value);
                }
            }
            else
                Value = LoadNoteValue(element.ElementValue("value"));
        }

        private static double LoadNoteValue(string value)
        {
            double result;
            if (NoteValues.ContainsKey(value))
                result = NoteValues[value];
            else if (double.TryParse(value, out result))
                ;
            else
                throw new Exception("Failed to load note value. Value neither a number or a predefined note value. value: " + value);
            return result;
        }

        public static Dictionary<string, double> LoadNoteValues(string path = "Assets/Music/Data.xml")
        {
            var result = new Dictionary<string, double>();
            var data = XElement.Load(path);
            foreach (var value in data.Element("noteValues").Elements())
                result[value.Element("name").Value] = double.Parse(value.ElementValue("value"));
            return result;
        }

        public double Duration(BPM bpm)
        {
            return Value * 60 / bpm.WholeNotesPerMinute;
        }

        public void Beep(BPM bpm)
        {
            if (Tone != null)
            {
                Console.Beep((int)Tone.Frequency, (int)(Value * 60000 / bpm.WholeNotesPerMinute));
            }
        }

        public override string ToString()
        {
            return "{Tone: " + Tone + " Value: " + Value + "}";
        }
    }
}
