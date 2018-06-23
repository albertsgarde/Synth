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
        public double WholeNotesPerMinute { get; private set; }

        public BPM(double beatsPerMinute, double noteValue = 0.25)
        {
            WholeNotesPerMinute = beatsPerMinute * noteValue;
        }

        public BPM(XElement element)
        {
            var beatsPerMinute = double.Parse(element.ElementValue("beatsPerMinute"));
            var noteValue = double.Parse(element.ElementValue("noteValue"));
            WholeNotesPerMinute = beatsPerMinute * noteValue;
        }

        public double BeatsPerMinute(double noteValue)
        {
            return WholeNotesPerMinute / noteValue;
        }

        public double NoteValue(double beatsPerMinute)
        {
            return WholeNotesPerMinute / beatsPerMinute;
        }
    }
}
