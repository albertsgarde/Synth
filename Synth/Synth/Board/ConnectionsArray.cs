using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SynthLib.Board
{
    public class ConnectionsArray : Connections
    {
        private Connection[] connections;

        public override int Count => connections.Length;

        public ConnectionsArray(int size, int freeConnectionsStart = 0) : base(freeConnectionsStart)
        {
            connections = new Connection[size];
            for (int i = 0; i < connections.Length; ++i)
                connections[i] = null;
        }

        public ConnectionsArray(XElement element) : base(element)
        {
            connections = new Connection[InvalidModuleSaveElementException.ParseInt(element.Element("count"))];
            for (int i = 0; i < connections.Length; ++i)
                connections[i] = null;
        }

        public override Connection this[int index] => connections[index];
        
        protected override Connection AddConnection(Connection con, int index)
        {
            var prevCon = connections[index];
            connections[index] = con;
            return prevCon;
        }

        protected override void RemoveConnection(Connection con, int index)
        {
            if (connections[con.SourceIndex] == con)
                connections[con.SourceIndex] = null;
        }

        protected override bool FirstFreeConnection(out int index)
        {
            for (int i = FreeConnectionsStart; i < connections.Length; ++i)
            {
                if (connections[i] == null)
                {
                    index = i;
                    return true;
                }
            }
            index = FreeConnectionsStart;
            return false;
        }

        public override IEnumerator<Connection> GetEnumerator()
        {
            for (int i = 0; i < connections.Length; ++i)
                yield return connections[i];
        }

        public override XElement ToXElement(string name)
        {
            return base.ToXElement(name);
        }
    }
}
