﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Board
{
    public class FlexConnections : Connections
    {
        private List<Connection> connections;

        public override int Count => connections.Count;

        public override Connection this[int index] => connections[index];

        public FlexConnections(int size = 0)
        {
            connections = new List<Connection>(size);
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
            for (int i = 0; i < connections.Count; ++i)
            {
                if (connections[i] == null)
                {
                    index = i;
                    return true;
                }
            }
            index = 0;
            return false;
        }

        public override IEnumerator<Connection> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}