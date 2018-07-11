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

        public override int FreeConnectionsStart { get; }

        public ConnectionsArray(int size, int freeConnectionsStart = 0)
        {
            if (size < freeConnectionsStart)
                throw new ArgumentException("Size must not be smaller than freeConnectionsStart or else there won't be room for the necessary connections.");
            connections = new Connection[size];
            FreeConnectionsStart = freeConnectionsStart;
            for (int i = 0; i < connections.Length; ++i)
                connections[i] = null;
        }

        public override Connection this[int index]
        {
            get
            {
                return connections[index];
            }
        }

        public override int Count
        {
            get
            {
                return connections.Length;
            }
        }
        
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
