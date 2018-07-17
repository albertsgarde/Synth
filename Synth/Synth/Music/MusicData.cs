using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff;
using System.Xml.Linq;

namespace SynthLib.Music
{
    public class MusicData
    {
        public IReadOnlyDictionary<string, ChordType> ChordTypes { get; }

        public IReadOnlyDictionary<string, Scale> Scales { get; }

        public IReadOnlyDictionary<string, double> NoteValues { get; }

        public IReadOnlyDictionary<string, Melody> Melodies { get; }

        public MusicData(string path, Action<string> log)
        {
            var chordTypes = new Dictionary<string, ChordType>();
            foreach (var element in XMLUtil.FindElements(path, e => e.Name == "chordTypes"))
            {
                foreach (var chordType in element.Elements())
                    chordTypes[chordType.ElementValue("name")] = new ChordType(chordType);
            }
            ChordTypes = chordTypes;

            var scales = new Dictionary<string, Scale>();
            foreach (var element in XMLUtil.FindElements(path, e => e.Name == "scales"))
            {
                foreach (var scale in element.Elements())
                    scales[scale.ElementValue("name")] = new Scale(scale, ChordTypes);
            }
            Scales = scales;

            var noteValues = new Dictionary<string, double>();
            foreach (var element in XMLUtil.FindElements(path, e => e.Name == "noteValues"))
            {
                foreach (var noteValue in element.Elements())
                {
                    string value = noteValue.ElementValue("value");
                    if (!double.TryParse(value, out double result) || result <= 0)
                        log.Invoke("Invalid noteValue " + value + ". Note values must be numbers and above 0.");
                    noteValues[noteValue.ElementValue("name")] = result;
                }
            }
            NoteValues = noteValues;

            var melodies = new Dictionary<string, Melody>();
            foreach (var element in XMLUtil.FindElements(path, e => e.Name == "melodies"))
            {
                foreach (var melody in element.Elements())
                    melodies[melody.ElementValue("name")] = new Melody(melody, noteValues);
            }
            Melodies = melodies;
        }
    }
}
