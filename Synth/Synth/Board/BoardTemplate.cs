using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynthLib.Board.Modules;
using System.Xml.Linq;
using Stuff;
using System.Collections;

namespace SynthLib.Board
{
    public class BoardTemplate : IEnumerable<Module>, ISaveable
    {
        private int moduleNum = -1;

        private readonly CrossReferencedDictionary<string, Module> modules;

        private readonly List<ConnectionTemplate> connections;

        public BoardTemplate(XElement element)
        {
            throw new NotImplementedException();
        }

        public BoardTemplate()
        {
            modules = new CrossReferencedDictionary<string, Module>();
            connections = new List<ConnectionTemplate>();
        }

        private class ConnectionTemplate : ISaveable
        {
            public string Source { get; }
            public int SourceIndex { get; }
            public string Dest { get; }
            public int DestIndex { get; }
            
            public ConnectionTemplate(string source, int sourceIndex, string dest, int destIndex)
            {
                Source = source;
                SourceIndex = sourceIndex;
                Dest = dest;
                DestIndex = destIndex;
            }

            public override string ToString()
            {
                return $"{{{Source}[{SourceIndex}] -> {Dest}[{DestIndex}]}}";
            }

            public XElement ToXElement(string name)
            {
                var element = new XElement(name);
                element.AddValue("source", Source);
                element.AddValue("sourceIndex", SourceIndex);
                element.AddValue("dest", Dest);
                element.AddValue("destIndex", DestIndex);
                return element;
            }
        }

        public void Add(Module mod)
        {
            modules["" + ++moduleNum] = mod;
        }

        public void AddConnection(Module source, Module dest, int sourceIndex = -1, int destIndex = -1)
        {
            if (!modules.Contains(source) || !modules.Contains(dest))
                throw new ArgumentException("Before creating a connection between two modules they must first be added individually.");

            connections.Add(new ConnectionTemplate(modules[source], sourceIndex, modules[dest], destIndex));
        }

        private void CreateConnection(ConnectionTemplate ct, CrossReferencedDictionary<string, Module> modules)
        {
            if (ct.SourceIndex == -1)
            {
                if (ct.DestIndex == -1)
                    Connections.NewConnection(modules[ct.Source], modules[ct.Dest]);
                else
                    Connections.NewConnection(modules[ct.Source], modules[ct.Dest], ct.DestIndex);
            }
            else
            {
                if (ct.DestIndex == -1)
                    Connections.NewConnection(modules[ct.Source], ct.SourceIndex, modules[ct.Dest]);
                else
                    Connections.NewConnection(modules[ct.Source], ct.SourceIndex, modules[ct.Dest], ct.DestIndex);
            }
        }

        public ModuleBoard CreateInstance(int sampleRate = 44100)
        {
            var boardModules = new CrossReferencedDictionary<string, Module>();
            foreach (var m in modules)
            {
                boardModules[m.Key] = m.Value.Clone();
            }

            foreach (var ct in connections)
            {
                CreateConnection(ct, boardModules);
            }


            var board = new ModuleBoard(boardModules.Keys2.ToArray(), sampleRate);

            return board;
        }

        public IEnumerator<Module> GetEnumerator()
        {
            return modules.Keys2.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public XElement ToXElement(string name)
        {
            var element = new XElement(name);

            var modulesElement = new XElement("modules");
            foreach (var mod in modules.Keys2)
                modulesElement.Add(mod.ToXElement(modules[mod]));
            element.Add(modulesElement);

            var connectionsElement = new XElement("connections");
            foreach (var con in connections)
                connectionsElement.Add(con.ToXElement("connection"));
            element.Add(connectionsElement);

            return element;
        }
    }
}
