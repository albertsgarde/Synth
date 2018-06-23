﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib.Board
{
    public class ConnectionsArray : Connections
    {
        private Connection[] connections;

        public ConnectionsArray(int size)
        {
            connections = new Connection[size];
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
            for (int i = 0; i < connections.Length; ++i)
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
            for (int i = 0; i < connections.Length; ++i)
                yield return connections[i];
        }
    }
}