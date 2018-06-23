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
        public ScaleContainer Scales { get; }



        public MusicData(string path)
        {
            var data = XDocument.Load(path);
            Scales = new ScaleContainer(data.Element("scales"));
        }

        public class ScaleContainer
        {
            private readonly Dictionary<string, Scale> scales;

            internal ScaleContainer(XElement element)
            {
                scales = new Dictionary<string, Scale>();
                foreach (var scale in element.Element("scales").Elements())
                    scales[scale.Element("name").Value] = new Scale(Chord.ChordTypes[scale.Element("rootChordType").Value], new int[] { 0 }.Union(scale.Elements("step").Select(e => int.Parse(e.Value))).ToArray());
            }

            public Scale this[string name]
            {
                get
                {
                    return scales[name];
                }
            }
        }

        public class MelodyContainer
        {
            private readonly Dictionary<string, Melody> melodies;

            internal MelodyContainer(XElement element)
            {
                melodies = new Dictionary<string, Melody>();
                foreach (var melody in element.Element("melodies").Elements())
                    melodies[melody.Element("name").Value] = new Melody(melody);
            }

            public Melody this[string name]
            {
                get
                {
                    return melodies[name];
                }
            }
        }
    }
}
