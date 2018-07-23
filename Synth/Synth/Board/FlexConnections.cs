using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml.Linq;

namespace SynthLib.Board
{
    public class FlexConnections : Connections
    {
        private List<Connection> connections;

        public override Connection this[int index] => connections[index];

        public override int Count => connections.Count;

        public FlexConnections(int size = 0, int freeConnectionsStart = 0) : base(freeConnectionsStart)
        {
            connections = new List<Connection>(size);
            if (Count < FreeConnectionsStart)
                throw new ArgumentException("Size must not be smaller than freeConnectionsStart or else there won't be room for the necessary connections.");
        }

        public FlexConnections(XElement element) : base(element)
        {
            connections = new List<Connection>();
        }

        protected override Connection AddConnection(Connection con, int index)
        {
            while (connections.Count <= index)
                connections.Add(null);
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
            for (int i = FreeConnectionsStart; i < connections.Count; ++i)
            {
                if (connections[i] == null)
                {
                    index = i;
                    return true;
                }
            }
            Debug.Assert(connections.Count >= FreeConnectionsStart);
            index = connections.Count;
            connections.Add(null);
            return true;
        }

        public override IEnumerator<Connection> GetEnumerator()
        {
            return connections.GetEnumerator();
        }
    }
}
