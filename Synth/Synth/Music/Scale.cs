using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Stuff;
using SynthLib.Music;

namespace SynthLib.Music
{
    public class Scale
    {
        public static readonly Scales Scales = new Scales();

        public int[] Steps { get; private set; }

        public ChordType RootChordType { get; private set; }

        public Scale(ChordType rootChordType, params int[] steps)
        {
            Steps = steps.OrderBy(i => i).ToArray();
            RootChordType = rootChordType;
        }

        public int Step(int step)
        {
            return Steps[step % Steps.Length];
        }
    }

    public class Scales
    {
        private Dictionary<string, Scale> scales;

        internal Scales()
        {
            scales = new Dictionary<string, Scale>();
            var data = XElement.Load("Assets/Music/Data.xml");
            foreach (var scale in data.Element("scales").Elements())
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
}
