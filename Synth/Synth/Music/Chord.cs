using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Stuff;

namespace SynthLib.Music
{
    public class Chord
    {
        public Tone Root { get; }

        public Tone BassNote { get; }

        public ChordType Type { get; }

        public int Inversion { get; }
        

        /// <summary>
        /// Initializes a Chord.
        /// </summary>
        /// <param name="root">The root of the chord. So for a C major, it would be C. For a F# minor in third inversion with A as bass, it would be F#. Ignores octave.</param>
        /// <param name="type">The type of the chord. Might be called colour. Diminished/minor/major/augmented.</param>
        /// <param name="bassNote">The bass note of the chord. Defaults to the root, but can be anything. Ignores octave, but the octave can always be retrieved at a later date.</param>
        /// <param name="inversion">The inversion of the chord.</param>
        public Chord(Tone root, ChordType type, Tone bassNote = null, int inversion = 1)
        {
            Root = root;
            BassNote = bassNote ?? root;
            Type = type;
            Inversion = inversion;
        }

        private static Dictionary<string, ChordType> LoadChordTypes()
        {
            var result = new Dictionary<string, ChordType>();
            var data = XElement.Load("Assets/Music/Data.xml");
            foreach (var chordType in data.Element("chordTypes").Elements())
                result[chordType.Element("name").Value] = new ChordType(chordType);
            return result;
        }

        /// <param name="octave">Which octave to place the root note in.</param>
        /// <returns>The notes in the chord without an extra bass note but in the inversion's order.</returns>
        public IEnumerable<Tone> Tones(int octave)
        {
            var root = Root.ToOctave(octave);
            for (int i = Inversion - 1; i < Type.Steps.Length; i++)
                yield return root - 12 + Type.Steps[i];
            for (int i = 0; i < Inversion - 1; i++)
                yield return root + Type.Steps[i];
        }
    }

    public class ChordType
    {
        public string Name { get; private set; }

        public int Inversions { get; private set; }

        public int[] Steps { get; private set; }

        public ChordType(XElement element)
        {
            Name = element.ElementValue("name");
            Inversions = int.Parse(element.ElementValue("inversions"));
            int steps = (element.Element("steps").Elements().Count(e => e.Value == "0") == 0 ? 1 : 0) + element.Element("steps").Elements().Count();
            Steps = new int[steps];
            int i = 0;
            if (element.Element("steps").Elements().Count(e => e.Value == "0") == 0)
                Steps[i++] = 0;
            foreach (var step in element.Element("steps").Elements())
                Steps[i++] = int.Parse(step.Value);
        }
    }
}
