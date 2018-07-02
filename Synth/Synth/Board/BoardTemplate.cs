using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynthLib.Board.Modules;
using System.Xml.Linq;
using Stuff;

namespace SynthLib.Board
{
    public class BoardTemplate
    {
        private int moduleNum = -1;

        private readonly CrossReferencedDictionary<int, Module> modules;

        private readonly List<ConnectionTemplate> connections;

        public BoardTemplate(XElement element)
        {
            throw new NotImplementedException();
        }

        public BoardTemplate()
        {
            modules = new CrossReferencedDictionary<int, Module>();
            connections = new List<ConnectionTemplate>();
        }

        private class ConnectionTemplate
        {
            public int Source { get; }
            public int SourceIndex { get; }
            public int Dest { get; }
            public int DestIndex { get; }
            
            public ConnectionTemplate(int source, int sourceIndex, int dest, int destIndex)
            {
                Source = source;
                SourceIndex = sourceIndex;
                Dest = dest;
                DestIndex = destIndex;
            }
        }

        public void AddModule(Module mod)
        {
            modules[++moduleNum] = mod;
        }

        public void AddConnection(Module source, Module dest, int sourceIndex = -1, int destIndex = -1)
        {
            if (!modules.Contains(source) || !modules.Contains(dest))
                throw new ArgumentException("Before creating a connection between two modules they must first be added individually.");

            connections.Add(new ConnectionTemplate(modules[source], sourceIndex, modules[dest], destIndex));
        }

        private void CreateConnection(ConnectionTemplate ct, CrossReferencedDictionary<int, Module> modules)
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
            var board = new ModuleBoard(44100);
            var boardModules = new CrossReferencedDictionary<int, Module>();
            foreach (var m in modules)
                boardModules[m.Key] = m.Value.Clone();

            board.Add(boardModules.Keys2.ToArray());
            foreach (var ct in connections)
                CreateConnection(ct, boardModules);

            return board;
        }
    }
}
