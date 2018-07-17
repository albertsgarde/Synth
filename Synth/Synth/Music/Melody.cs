using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SynthLib.Music
{
    public class Melody : IEnumerable
    {
        private readonly List<Note> notes;

        public Melody(XElement element, IReadOnlyDictionary<string, double> noteValues)
        {
            notes = new List<Note>();
            foreach (var note in element.Elements("note"))
                notes.Add(new Note(note, noteValues));
        }

        public IEnumerator GetEnumerator()
        {
            return notes.GetEnumerator();
        }

        public void Play(BPM bpm)
        {
            foreach (var note in notes)
                note.Beep(bpm);
        }

        /// <summary>
        /// The duration of the melody in whole notes.
        /// </summary>
        public double Length
        {
            get
            {
                return notes.Sum(note => note.Value);
            }
        }

        public override string ToString()
        {
            string result = "";
            foreach (var note in notes)
                result += note.Tone;
            return result;
        }
    }
}
