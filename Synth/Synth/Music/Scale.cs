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
    public class Scale : ISaveable
    {
        public int[] Steps { get; }

        public string Name { get; }

        public ChordType RootChordType { get; }

        public Scale(string name, ChordType rootChordType, params int[] steps)
        {
            Name = name;
            Steps = steps.OrderBy(i => i).ToArray();
            RootChordType = rootChordType;
        }

        public Scale(XElement element, IReadOnlyDictionary<string, ChordType> chordTypes)
        {
            Name = element.ElementValue("name");
            RootChordType = chordTypes[element.ElementValue("rootChordType")];
            new int[] { 0 }.Union(element.Elements("step").Select(e => int.Parse(e.Value))).ToArray();
        }

        public int Step(int step)
        {
            return Steps[step % Steps.Length];
        }

        public XElement ToXElement(string name)
        {
            var steps = new XElement("steps");
            foreach (var step in Steps)
                steps.Add(new XElement("step", step));
            return new XElement(name,
                new XElement("name", Name),
                steps);
        }
    }
}
