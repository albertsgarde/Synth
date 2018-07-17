using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Stuff;

namespace SynthLib.Music
{
    public class BPM
    {
        public double QuarterNotesPerMinute { get; }

        public BPM(double beatsPerMinute, double noteValue = Note.QUARTER_NOTE)
        {
            QuarterNotesPerMinute = beatsPerMinute * noteValue;
        }

        public BPM(XElement element)
        {
            var beatsPerMinute = double.Parse(element.ElementValue("beatsPerMinute"));
            var noteValue = double.Parse(element.ElementValue("noteValue"));
            QuarterNotesPerMinute = beatsPerMinute * noteValue;
        }

        public double BeatsPerMinute(double noteValue)
        {
            return QuarterNotesPerMinute / noteValue;
        }

        public double NoteValue(double beatsPerMinute)
        {
            return QuarterNotesPerMinute / beatsPerMinute;
        }
    }
}
